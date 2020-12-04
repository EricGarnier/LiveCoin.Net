using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using LiveCoin.Net.Objects;
using LiveCoin.Net.Objects.SocketObjects;
using LiveCoin.Net.Sockets;
using LiveCoin.Net.Test;
using Newtonsoft.Json.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

namespace LiveCoin.Net
{
	/// <summary>
	/// Client providing access to the LiveCoin websocket Api
	/// </summary>
	public class LiveCoinSocketClient : SocketClient
	{
		#region fields
		private static LiveCoinSocketClientOptions _defaultOptions = new LiveCoinSocketClientOptions();
		private static LiveCoinSocketClientOptions DefaultOptions => _defaultOptions.Copy();
		private int _timeToLive;

		#endregion
		private readonly string _baseAddress;
		#region constructor/destructor

		/// <summary>
		/// Create a new instance of BinanceSocketClient with default options
		/// </summary>
		public LiveCoinSocketClient() : this(DefaultOptions)
		{
		}

		/// <summary>
		/// Create a new instance of BinanceSocketClient using provided options
		/// </summary>
		/// <param name="options">The options to use for this client</param>
		public LiveCoinSocketClient(LiveCoinSocketClientOptions options) : base(options, options.ApiCredentials == null ? null : new LiveCoinAuthenticationProvider(options.ApiCredentials, ArrayParametersSerialization.MultipleValues))
		{
			_baseAddress = options.BaseAddress.TrimEnd('/');
			_timeToLive = options.TimeToLive;
			SocketFactory = new BinarySocketFactory();
			ContinueOnQueryResponse = false;
		}
		#endregion

		#region method
		/// <inheritdoc/>
		protected override SocketSubscription AddHandler<T>(object? request, string? identifier, bool userSubscription, SocketConnection connection, Action<T> dataHandler)
		{
			void InternalHandler(SocketConnection socketWrapper, JToken data)
			{
				T response = GetWsMessage<T>(data);
				if (response == null)
				{
					var m = GetWsResponse(data);
					log.Write(LogVerbosity.Warning, $"Failed to deserialize data into type {typeof(T)} ResponseType:{m?.Meta?.ResponseType}, Token:{m?.Meta?.Token} Msg.Length:{m?.Msg.Length}");
					return;
				}
				dataHandler(response);
			}

			var handler = request == null
				? SocketSubscription.CreateForIdentifier(identifier!, userSubscription, InternalHandler)
				: SocketSubscription.CreateForRequest(request, userSubscription, InternalHandler);
			connection.AddHandler(handler);
			return handler;
		}

		private BinaryData BuildRequest<T>(T? request, string token, WsRequestMetaData.WsRequestMsgType requestType, bool signed) where T : class
		{
			byte[] msg = new byte[0];
			var meta = new WsRequestMetaData()
			{
				RequestType = requestType,
				Token = token
			};
			if (request is IExpireControl expireControl)
			{
				expireControl.ExpireControl = new RequestExpired
				{
					Now = DateTime.UtcNow,
					Ttl = _timeToLive
				};
			}
			if (request != null)
			{
				using (MemoryStream msgStream = new MemoryStream())
				{
					ProtoBuf.Serializer.Serialize(msgStream, request);
					msg = msgStream.ToArray();
				}
			}
			WsRequest wsRequest = new WsRequest()
			{
				Meta = meta,
				Msg = msg
			};
			if (signed)
			{
				wsRequest.Meta.Sign = authProvider?.Sign(wsRequest.Msg);
			}
			var result = new BinaryData();
			using (var requestStream = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize(requestStream, wsRequest);
				result.Data = System.Convert.ToBase64String(requestStream.ToArray());
			}
			result.Token = token;
			return result;
		}
		public async Task<CallResult<PongResponse>> Ping()
		{
			var binaryRequest = BuildRequest<object>(null, nameof(Ping) + NextId().ToString(), WsRequestMetaData.WsRequestMsgType.PingRequest, false);
			binaryRequest.ExpectedResponseMsgType = WsResponseMetaData.WsResponseMsgType.PongResponse;
			return await Query<PongResponse>(_baseAddress, binaryRequest, false);
		}
		public async Task<CallResult<CancelLimitOrderResponse>> CancelLimitOrder(string symbol, long orderId)
		{
			var message = new CancelLimitOrderRequest()
			{
				CurrencyPair = symbol,
				Id = orderId,
			};
			var binaryRequest = BuildRequest(message, nameof(CancelLimitOrder) + NextId().ToString(), WsRequestMetaData.WsRequestMsgType.CancelLimitOrder, true);
			binaryRequest.ExpectedResponseMsgType = WsResponseMetaData.WsResponseMsgType.CancelLimitOrderResponse;
			return await Query<CancelLimitOrderResponse>(_baseAddress, binaryRequest, true);
		}

		private static bool HandleTickerNotifyMessage(BinaryData request, JToken jtoken, WsResponse header)
		{
			if (string.IsNullOrWhiteSpace(header.Meta.Token) && header.Meta.ResponseType == WsResponseMetaData.WsResponseMsgType.TickerNotify)
			{
				TickerNotification? tickerNotification = GetWsMessage<TickerNotification>(jtoken);
				if (tickerNotification?.CurrencyPair == request.CurrencyPair)
				{
					return true;
				}
			}
			return false;
		}
		public async Task<CallResult<UpdateSubscription>> SubscribeTicker(string symbol, Action<TickerNotification> action)
		{
			var message = new SubscribeTickerChannelRequest
			{
				CurrencyPair = symbol
			};
			var binaryRequest = BuildRequest(message, nameof(SubscribeTicker) + NextId(), WsRequestMetaData.WsRequestMsgType.SubscribeTicker, false);
			binaryRequest.ExpectedResponseMsgType = WsResponseMetaData.WsResponseMsgType.TickerChannelSubscribed;
			binaryRequest.ChannelType = UnsubscribeRequest.ChannelType.Ticker;
			binaryRequest.CurrencyPair = symbol;
			binaryRequest.HandleMessageData = HandleTickerNotifyMessage;
			return await this.Subscribe<TickerNotification>(_baseAddress, binaryRequest, binaryRequest.Token, false, action);
		}


		/// <inheritdoc/>
		protected override async Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
		{
			if (authProvider == null)
				return new CallResult<bool>(false, new NoApiCredentialsError());
			var message = new LoginRequest
			{
				ApiKey = authProvider.Credentials?.Key?.GetString(),
			};
			var binaryRequest = BuildRequest(message, nameof(AuthenticateSocket) + NextId(), WsRequestMetaData.WsRequestMsgType.Login, true);
			var result = new CallResult<bool>(false, new ServerError("No response from server"));
			await s.SendAndWait(binaryRequest, ResponseTimeout, data =>
			{
				var response = GetWsResponse(data);
				if (response.Meta.ResponseType == WsResponseMetaData.WsResponseMsgType.LoginResponse && response.Meta.Token == binaryRequest.Token)
				{
					log.Write(LogVerbosity.Debug, "Authorization completed");
					result = new CallResult<bool>(true, null);
					return true;
				}
				return false;
			});
			return result;
		}

		/// <inheritdoc/>
		protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
		{
			var response = GetWsResponse(data);
			var fullRequest = (BinaryData)request;
			if (response.Meta.Token == ((BinaryData)request).Token)
			{
				if (response.Meta.ResponseType == fullRequest.ExpectedResponseMsgType)
				{
					callResult = new CallResult<T>(GetWsMessage<T>(data), null);
					return true;
				}
				else if (response.Meta.ResponseType == WsResponseMetaData.WsResponseMsgType.Error)
				{
					var error = GetWsMessage<ErrorResponse>(data);
					if (error != null)
					{
						callResult = new CallResult<T>(default(T), new ServerError(error.Code, error.Message ?? "An error occured"));
						return true;
					}
				}
				callResult = new CallResult<T>(default(T), new ServerError(-1, $"Unknown message type received {response.Meta.ResponseType}"));
				return true;

			}
			callResult = new CallResult<T>(default(T), null);
			return false;
		}
		static ConditionalWeakTable<JToken, WsResponse> ExtendedWsResponse = new ConditionalWeakTable<JToken, WsResponse>();
		static ConditionalWeakTable<JToken, WsResponse>.CreateValueCallback ExtendedWsResponseCreateValueCallback = new ConditionalWeakTable<JToken, WsResponse>.CreateValueCallback(ExtendedWsResponseCreateValueFunction);
		private static WsResponse ExtendedWsResponseCreateValueFunction(JToken key)
		{
			var strData = key.Value<string>("Data");
			using (MemoryStream responseStream = new MemoryStream(System.Convert.FromBase64String(strData)))
			{
				WsResponse response = ProtoBuf.Serializer.Deserialize<WsResponse>(responseStream);
				return response;
			};
		}
		static ConditionalWeakTable<JToken, Object> ExtendedMessage = new ConditionalWeakTable<JToken, object>();
		static ConditionalWeakTable<JToken, Object>.CreateValueCallback ExtendedWsMessageCreateValueCallback = new ConditionalWeakTable<JToken, Object>.CreateValueCallback(ExtendedMessageCreateValueFunction);
		private static T DecodeMessage<T>(byte[] msg)
		{
			using (MemoryStream responseStream = new MemoryStream(msg))
			{
				T response = ProtoBuf.Serializer.Deserialize<T>(responseStream);
				return response;
			};
		}

		private static object? ExtendedMessageCreateValueFunction(JToken key)
		{
			var response = GetWsResponse(key);
			var msg = response.Msg;
			switch (response.Meta.ResponseType)
			{
				case WsResponseMetaData.WsResponseMsgType.TickerChannelSubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.OrderBookRawChannelSubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.OrderBookChannelSubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.TradeChannelSubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.CandleChannelSubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.ChannelUnsubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.Error:
					return DecodeMessage<ErrorResponse>(msg);
				case WsResponseMetaData.WsResponseMsgType.TickerNotify:
					return DecodeMessage<TickerNotification>(msg);
				case WsResponseMetaData.WsResponseMsgType.OrderBookRawNotify:
					break;
				case WsResponseMetaData.WsResponseMsgType.OrderBookNotify:
					break;
				case WsResponseMetaData.WsResponseMsgType.TradeNotify:
					break;
				case WsResponseMetaData.WsResponseMsgType.CandleNotify:
					break;
				case WsResponseMetaData.WsResponseMsgType.LoginResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.PutLimitOrderResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.CancelLimitOrderResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.BalanceResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.BalancesResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.LastTradesResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.TradesResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.ClientOrdersResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.ClientOrderResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.CommissionResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.CommissionCommonInfoResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.TradeHistoryResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.MarketOrderResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.WalletAddressResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.WithdrawalCoinResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.WithdrawalPayeerResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.WithdrawalCapitalistResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.WithdrawalAdvcashResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.PrivateOrderRawChannelSubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.PrivateTradeChannelSubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.PrivateOrderRawNotify:
					break;
				case WsResponseMetaData.WsResponseMsgType.PrivateTradeNotify:
					break;
				case WsResponseMetaData.WsResponseMsgType.PrivateChannelUnsubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.WithdrawalYandexResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.WithdrawalQiwiResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.WithdrawalCardResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.WithdrawalMastercardResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.WithdrawalPerfectmoneyResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.VoucherMakeResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.VoucherAmountResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.VoucherRedeemResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.CancelOrdersResponse:
					break;
				case WsResponseMetaData.WsResponseMsgType.PongResponse:
					return DecodeMessage<PongResponse>(msg);
				case WsResponseMetaData.WsResponseMsgType.BalanceChangeChannelSubscribed:
					break;
				case WsResponseMetaData.WsResponseMsgType.BalanceChangeNotify:
					break;
				default:
					break;
			}
			return null;
		}
		private static WsResponse GetWsResponse(JToken key)
		{
			return ExtendedWsResponse.GetValue(key, ExtendedWsResponseCreateValueCallback);
		}
		private static T GetWsMessage<T>(JToken key)  //where T : class
		{
			T result = default(T);
			object r = ExtendedMessage.GetValue(key, ExtendedMessageCreateValueFunction);
			if (r != null && typeof(T).IsAssignableFrom(r.GetType()))
			{
				result = (T)r;
			}
			return result;
		}

		/// <inheritdoc/>
		protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken jmessage, out CallResult<object>? callResult)
		{
			var response = GetWsResponse(jmessage);
			var fullRequest = (BinaryData)request;
			if (response.Meta.Token == ((BinaryData)request).Token)
			{
				if (response.Meta.ResponseType == fullRequest.ExpectedResponseMsgType)
				{
					callResult = new CallResult<object>(true, null);
					return true;
				}
				else if (response.Meta.ResponseType == WsResponseMetaData.WsResponseMsgType.Error)
				{
					var error = GetWsMessage<ErrorResponse>(jmessage);
					if (error != null)
					{
						callResult = new CallResult<object>(false, new ServerError(error.Code, error.Message ?? "An error occured"));
						return true;
					}
				}
				callResult = new CallResult<object>(false, new ServerError(-1, $"Unknown message type received {response.Meta.ResponseType}"));
				return true;

			}
			callResult = new CallResult<object>(false, null);
			return false;
		}

		/// <inheritdoc/>
		protected override bool MessageMatchesHandler(JToken message, object request)
		{
			var response = GetWsResponse(message);
			var fullRequest = (BinaryData)request;
			if (!string.IsNullOrEmpty(response.Meta.Token))
			{
				return false;
			}
			else
			{
				return fullRequest.HandleMessageData?.Invoke(fullRequest, message, response) == true;
			}
		}

		/// <inheritdoc/>
		protected override bool MessageMatchesHandler(JToken message, string identifier)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		protected override async Task<bool> Unsubscribe(SocketConnection connection, SocketSubscription s)
		{
			var fullRequest = (BinaryData?)s.Request;
			if ( fullRequest == null || fullRequest?.ChannelType == null || fullRequest?.CurrencyPair == null)
			{
				return false;
			}
			var request = new UnsubscribeRequest
			{
				Channel_Type = fullRequest.ChannelType??((UnsubscribeRequest.ChannelType)(-1)),
				CurrencyPair = fullRequest.CurrencyPair
			};
			var binaryRequest = BuildRequest(request, nameof(Unsubscribe) + NextId().ToString(), WsRequestMetaData.WsRequestMsgType.Unsubscribe, false);
			binaryRequest.ExpectedResponseMsgType = WsResponseMetaData.WsResponseMsgType.ChannelUnsubscribed;
			var res = await QueryAndWait<ChannelUnsubscribedResponse>(connection, binaryRequest);
			return res.Success;
		}
		#endregion
	}
}
