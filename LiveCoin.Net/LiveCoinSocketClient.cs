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
			_baseAddress = options.BaseAddress;
			SocketFactory = new BinarySocketFactory();
			ContinueOnQueryResponse = false;
		}
		#endregion

		#region method


		protected override SocketSubscription AddHandler<T>(object? request, string? identifier, bool userSubscription, SocketConnection connection, Action<T> dataHandler)
		{
			void InternalHandler(SocketConnection socketWrapper, JToken data)
			{
				//if (typeof(T) == typeof(string))
				//{
				//	dataHandler((T)Convert.ChangeType(data.ToString(), typeof(T)));
				//	return;
				//}
				var response = GetWsMessage<TickerNotification>(data);

				//var desResult = Deserialize<T>(data, false);
				//if (!desResult)
				if (!(response is T))
				{
					//					log.Write(LogVerbosity.Warning, $"Failed to deserialize data into type {typeof(T)}: {desResult.Error}");
					log.Write(LogVerbosity.Warning, $"Failed to deserialize data into type {typeof(T)}: ???");
					return;
				}

				//dataHandler(desResult.Data);
				T res = (T)Convert.ChangeType(response, typeof(T));
				dataHandler(res);
			}

			var handler = request == null
				? SocketSubscription.CreateForIdentifier(identifier!, userSubscription, InternalHandler)
				: SocketSubscription.CreateForRequest(request, userSubscription, InternalHandler);
			connection.AddHandler(handler);
			return handler;
		}

		public async Task<CallResult<PongResponse>> TestPing()
		{
			//			Query
			string token;
			byte[] msg = new byte[0];
			WsRequestMetaData.WsRequestMsgType requestType = WsRequestMetaData.WsRequestMsgType.PingRequest;
			token = "Ping" + Guid.NewGuid().ToString();
			var meta = new WsRequestMetaData()
			{
				RequestType = requestType,
				Token = token
			};
			WsRequest wsRequest = new WsRequest()
			{
				Meta = meta,
				Msg = msg
			};
			var request = new BinaryData();
			using (var requestStream = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize(requestStream, wsRequest);
				request.Data = System.Convert.ToBase64String(requestStream.ToArray());
				request.Token = token;
				request.ExpectedResponseMsgType = WsResponseMetaData.WsResponseMsgType.PongResponse;
			}
			return await Query<PongResponse>("wss://ws.api.livecoin.net/ws/beta2", request, false);

		}
		private static byte[] Serialize<T>(T value)
		{
			using (var requestStream = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize(requestStream, value);
				return	requestStream.ToArray();
			}
		}
		public async Task<CallResult<UpdateSubscription>> test(string symbol)
		{
			string token;
			byte[] msg;
			WsRequestMetaData.WsRequestMsgType requestType;
			var message = new SubscribeTickerChannelRequest
			{
				CurrencyPair = symbol
			};
			using (MemoryStream msgStream = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize(msgStream, message);
				msg = msgStream.ToArray();
				requestType = WsRequestMetaData.WsRequestMsgType.SubscribeTicker;
				token = "Ticker_" + "BTC/USD" + " channel" + Guid.NewGuid().ToString();
			}

			var meta = new WsRequestMetaData()
			{
				RequestType = requestType,
				Token = token
			};

			WsRequest wsRequest = new WsRequest()
			{
				Meta = meta,
				Msg = msg
			};
			var request = new BinaryData();
			using (var requestStream = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize(requestStream, wsRequest);
				request.Data = System.Convert.ToBase64String(requestStream.ToArray());
				request.Token = token;
				request.ExpectedResponseMsgType = WsResponseMetaData.WsResponseMsgType.TickerChannelSubscribed;
				request.HandleMessageData = (jtoken, header) =>
				{
					if (string.IsNullOrWhiteSpace(header.Meta.Token) && header.Meta.ResponseType == WsResponseMetaData.WsResponseMsgType.TickerNotify)
					{
						TickerNotification? tickerNotification = GetWsMessage<TickerNotification>(jtoken);
						if (tickerNotification?.CurrencyPair == symbol)
						{
							return true;
						}
					}
					return false;
				};
			}
			var t = await this.Subscribe<TickerNotification>("wss://ws.api.livecoin.net/ws/beta2", request, token, false,
				(o) =>
				{

					Console.WriteLine($"Ticker : {o.CurrencyPair} symbol:{symbol}");
					foreach (var t in o.Datas)
					{
						Console.WriteLine($"  BestAsk:{t.BestAsk} BestBid:{t.BestBid} Timestamp:{t.Timestamp}");
					}
				});
			return t;
		}


		/// <inheritdoc/>
		protected override Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
		{
			//Todo : ega : envoyer un login, et attendre la réponse.
			throw new NotImplementedException();
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

		private static object ExtendedMessageCreateValueFunction(JToken key)
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
		private static T GetWsMessage<T>(JToken key) // where T : class
		{
			T result = default(T);
			object r = ExtendedMessage.GetValue(key, ExtendedMessageCreateValueFunction);
			if (r != null && typeof(T).IsAssignableFrom(r.GetType()))
			{
				result = (T)r;
			}
			return result;
		}
		/*
		*/
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
				return fullRequest.HandleMessageData?.Invoke(message, response) == true;
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
			if (fullRequest == null)
			{
				return false;
			}
			var wsRequest = new WsRequest
			{
				Meta = new WsRequestMetaData
				{
					RequestType = WsRequestMetaData.WsRequestMsgType.Unsubscribe,
					Token = "unsubscribe" + Guid.NewGuid().ToString(),
				},
				Msg = Serialize(new UnsubscribeRequest
				{
					Channel_Type = UnsubscribeRequest.ChannelType.Ticker,
					CurrencyPair = "BTC/USD"
				})
			};
			var request = new BinaryData
			{
				Data = System.Convert.ToBase64String(Serialize(wsRequest)),
				ExpectedResponseMsgType = WsResponseMetaData.WsResponseMsgType.ChannelUnsubscribed,
				Token = wsRequest.Meta.Token,
			};
			var res = await QueryAndWait<ChannelUnsubscribedResponse>(connection, request);
			return res.Success;
		}
		#endregion
	}
}
