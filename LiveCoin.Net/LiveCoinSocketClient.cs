using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using LiveCoin.Net.Enums;
using LiveCoin.Net.Objects;
using LiveCoin.Net.Objects.SocketObjects;
using LiveCoin.Net.Sockets;
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
		private readonly int _timeToLive;

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
		public LiveCoinSocketClient(LiveCoinSocketClientOptions options) : base("LiveCoin", options, options.ApiCredentials == null ? null : new LiveCoinAuthenticationProvider(options.ApiCredentials))
		{
			_baseAddress = options.BaseAddress.TrimEnd('/');
			_timeToLive = options.TimeToLive;
			SocketFactory = new BinarySocketFactory();
			ContinueOnQueryResponse = true;
		}
		#endregion

		#region method

		/// <summary>
		/// Set the default options to be used when creating new clients
		/// </summary>
		/// <param name="options"></param>
		public static void SetDefaultOptions(LiveCoinSocketClientOptions options)
		{
			_defaultOptions = options;
		}

		/// <summary>
		/// Set the API key and secret
		/// </summary>
		/// <param name="apiKey">The api key</param>
		/// <param name="apiSecret">The api secret</param>
		public void SetApiCredentials(string apiKey, string apiSecret)
		{
			SetAuthenticationProvider(new LiveCoinAuthenticationProvider(new ApiCredentials(apiKey, apiSecret)));
		}


		/// <inheritdoc/>
		protected override SocketSubscription AddHandler<T>(object? request, string? identifier, bool userSubscription, SocketConnection connection, Action<T> dataHandler)
		{
			void InternalHandler(SocketConnection socketWrapper, JToken data)
			{
				T response = GetWsMessage<T>(data);
				if (response == null)
				{
					var m = GetWsResponse(data);
					log.Write(LogVerbosity.Warning, $"Failed to deserialize data into type {typeof(T)} ResponseType:{m?.Meta?.ResponseType}, Token:{m?.Meta?.Token} Msg.Length:{m?.Msg?.Length}");
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

		private BinaryData BuildRequest<T>(T? request, string token, WsRequestMsgType requestType, bool signed) where T : class
		{
			var result = new BinaryData();
			result._buildData = () =>
			{
				byte[] msg = Array.Empty<byte>();
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
				using (var requestStream = new MemoryStream())
				{
					ProtoBuf.Serializer.Serialize(requestStream, wsRequest);
					return System.Convert.ToBase64String(requestStream.ToArray());
				}
			};
			result.Token = token;
			return result;
		}


		/// <summary>
		/// Ping command
		/// </summary>
		/// <returns>Pong response from the server and elapsed millisecond</returns>
		public async Task<CallResult<(PongResponse? serverResponse, long elapsedMilliseconds)>> PingAsync()
		{
			var binaryRequest = BuildRequest<object>(null, nameof(Ping) + NextId().ToString(), WsRequestMsgType.PingRequest, false);
			var sw = Stopwatch.StartNew();
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.PongResponse;
			var result = await Query<PongResponse>(_baseAddress, binaryRequest, false);
			sw.Stop();
			return new CallResult<(PongResponse? serverResponse, long elapsedMilliseconds)>(result.Success ? (result.Data, sw.ElapsedMilliseconds) : (null, 0), result.Error);
		}
		///<inheritdoc cref="PingAsync"/>
		public CallResult<(PongResponse? serverResponse, long elapsedMilliseconds)> Ping() => PingAsync().Result;
		/// <summary>
		/// Cancel limit order
		/// </summary>
		/// <param name="symbol">currency pair</param>
		/// <param name="orderId">id of the order to cancel</param>
		/// <returns></returns>
		public async Task<CallResult<CancelLimitOrderResponse>> CancelLimitOrderAsync(string symbol, long orderId)
		{
			symbol.ValidateLiveCoinSymbol();
			var message = new CancelLimitOrderRequest()
			{
				CurrencyPair = symbol,
				Id = orderId,
			};
			var binaryRequest = BuildRequest(message, nameof(CancelLimitOrder) + NextId().ToString(), WsRequestMsgType.CancelLimitOrder, true);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.CancelLimitOrderResponse;
			return await Query<CancelLimitOrderResponse>(_baseAddress, binaryRequest, true);
		}
		///<inheritdoc cref="CancelLimitOrderAsync"/>
		public CallResult<CancelLimitOrderResponse> CancelLimitOrder(string symbol, long orderId) => CancelLimitOrderAsync(symbol, orderId).Result;

		/// <summary>
		/// Cancel orders
		/// </summary>
		/// <param name="symbols">currency pairs</param>
		/// <returns></returns>
		public async Task<CallResult<CancelOrdersResponse>> CancelOrdersAsync(IEnumerable<string> symbols)
		{
			symbols.ValidateNotNull(nameof(symbols));
			var message = new CancelOrdersRequest()
			{
			};
			message.CurrencyPairs.AddRange(symbols);
			var binaryRequest = BuildRequest(message, nameof(CancelOrders) + NextId().ToString(), WsRequestMsgType.CancelOrders, true);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.CancelOrdersResponse;
			return await Query<CancelOrdersResponse>(_baseAddress, binaryRequest, true);
		}
		///<inheritdoc cref="CancelLimitOrderAsync"/>
		public CallResult<CancelOrdersResponse> CancelOrders(IEnumerable<string> symbols) => CancelOrdersAsync(symbols).Result;

		/// <summary>
		/// get client orders
		/// Weight is 1 point when currency is given, 5 points for "all currencies"
		/// </summary>
		/// <param name="symbol">currency pair</param>
		/// <param name="status">order status filter</param>
		/// <param name="issuedFrom">issued from</param>
		/// <param name="issuedTo">issued to</param>
		/// <param name="orderType">Order type filter</param>
		/// <param name="startRow">start row number</param>
		/// <param name="endRow">end row number</param>
		/// <returns>list of orders</returns>
		public async Task<CallResult<ClientOrdersResponse>> ClientOrdersAsync(string? symbol, OrderStatusFilter? status, DateTime? issuedFrom, DateTime? issuedTo, OrderBidAskType? orderType, int? startRow, int? endRow)
		{
			symbol?.ValidateLiveCoinSymbol();
			var message = new ClientOrdersRequest()
			{
				CurrencyPair = symbol,
				Status = status == (OrderStatusFilter.All) ? null : status,
				IssuedFrom = issuedFrom,
				IssuedTo = issuedTo,
				OrderType = orderType,
				StartRow = startRow,
				EndRow = endRow,
			};
			var binaryRequest = BuildRequest(message, nameof(ClientOrders) + NextId().ToString(), WsRequestMsgType.ClientOrders, true);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.ClientOrdersResponse;
			return await Query<ClientOrdersResponse>(_baseAddress, binaryRequest, true);
		}
		///<inheritdoc cref="ClientOrdersAsync"/>
		public CallResult<ClientOrdersResponse> ClientOrders(string? symbol, OrderStatusFilter? status, DateTime? issuedFrom, DateTime? issuedTo, OrderBidAskType? orderType, int? startRow, int? endRow) => ClientOrdersAsync(symbol, status, issuedFrom, issuedTo, orderType, startRow, endRow).Result;

		/// <summary>
		/// get one client order
		/// </summary>
		/// <param name="symbol">currency pair</param>
		/// <param name="orderId">order id</param>
		/// <returns>order detail</returns>
		public async Task<CallResult<ClientOrderResponse>> ClientOrderAsync(string symbol, long orderId)
		{
			symbol?.ValidateLiveCoinSymbol();
			var message = new ClientOrderRequest()
			{
				CurrencyPair = symbol,
				OrderId = orderId
			};
			var binaryRequest = BuildRequest(message, nameof(ClientOrder) + NextId().ToString(), WsRequestMsgType.ClientOrder, true);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.ClientOrderResponse;
			return await Query<ClientOrderResponse>(_baseAddress, binaryRequest, true);
		}
		///<inheritdoc cref="ClientOrderAsync"/>
		public CallResult<ClientOrderResponse> ClientOrder(string symbol, long orderId) => ClientOrderAsync(symbol, orderId).Result;
		/// <summary>
		/// Put a limit order - private api
		/// Weight is 1 point
		/// </summary>
		/// <param name="symbol">currency pair</param>
		/// <param name="amount">order amount</param>
		/// <param name="orderType">order type</param>
		/// <param name="price">order price</param>
		/// <returns></returns>
		public async Task<CallResult<PutLimitOrderResponse>> PutLimitOrderAsync(string symbol, OrderBidAskType orderType, decimal price, decimal amount)
		{
			symbol.ValidateLiveCoinSymbol();
			var message = new PutLimitOrderRequest()
			{
				CurrencyPair = symbol,
				Amount = amount,
				OrderType = orderType,
				Price = price
			};
			var binaryRequest = BuildRequest(message, nameof(PutLimitOrder) + NextId().ToString(), WsRequestMsgType.PutLimitOrder, true);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.PutLimitOrderResponse;
			return await Query<PutLimitOrderResponse>(_baseAddress, binaryRequest, true);
		}
		///<inheritdoc cref="PutLimitOrderAsync"/>
		public CallResult<PutLimitOrderResponse> PutLimitOrder(string symbol, OrderBidAskType orderType, decimal price, decimal amount) => PutLimitOrderAsync(symbol, orderType, price, amount).Result;

		private static bool HandleTickerNotifyMessage(BinaryData request, JToken jtoken, WsResponse header)
		{
			if ((string.IsNullOrWhiteSpace(header.Meta?.Token) && header?.Meta?.ResponseType == WsResponseMsgType.TickerNotify) || header?.Meta?.ResponseType == WsResponseMsgType.TickerChannelSubscribed)
			{
				TickerNotification? tickerNotification = GetWsMessage<TickerNotification>(jtoken);
				if (tickerNotification?.CurrencyPair == request.CurrencyPair)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Subscribe to ticker notification
		/// </summary>
		/// <param name="symbol">Currency pair</param>
		/// <param name="frequency">send rate will be limited to one message per frequency seconds. Minimum frequency is 0.1
		/// if omitted, rate is not limited (you will get all changes just in time)
		/// </param>
		/// <param name="action">ticker processing</param>
		/// <returns>subscription result</returns>
		public async Task<CallResult<UpdateSubscription>> SubscribeTickerAsync(string symbol, float? frequency, Action<TickerNotification> action)
		{
			symbol.ValidateLiveCoinSymbol();
			var message = new SubscribeTickerChannelRequest
			{
				CurrencyPair = symbol,
				Frequency = frequency
			};
			var binaryRequest = BuildRequest(message, nameof(SubscribeTicker) + NextId(), WsRequestMsgType.SubscribeTicker, false);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.TickerChannelSubscribed;
			binaryRequest.ChannelType = ChannelType.Ticker;
			binaryRequest.CurrencyPair = symbol;
			binaryRequest.HandleMessageData = HandleTickerNotifyMessage;
			return await this.Subscribe<TickerNotification>(_baseAddress, binaryRequest, null, false, action);
		}
		///<inheritdoc cref="SubscribeTickerAsync"/>
		public CallResult<UpdateSubscription> SubscribeTicker(string symbol, float? frequency, Action<TickerNotification> action) => SubscribeTickerAsync(symbol, frequency, action).Result;


		private static bool HandleOrderBookRawNotificationMessage(BinaryData request, JToken jtoken, WsResponse header)
		{
			if ((string.IsNullOrWhiteSpace(header?.Meta?.Token) && header?.Meta?.ResponseType == WsResponseMsgType.OrderBookRawNotify) || (header?.Meta?.ResponseType == WsResponseMsgType.OrderBookChannelSubscribed))
			{
				OrderBookRawNotification? orderBookNotification = GetWsMessage<OrderBookRawNotification>(jtoken);
				if (orderBookNotification?.CurrencyPair == request.CurrencyPair)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Subscribe to orderbook raw notification
		/// </summary>
		/// <param name="symbol">the currency pair</param>
		/// <param name="depth">depth of orderbook, which will be sent in an subscription answer
		///  0 meens 'do not send orderbook snapshot'
		///  if omitted, full orderbook snaphot will be sent
		/// </param>
		/// <param name="action">orderbook raw notification processing</param>
		/// <returns>subscription result</returns>
		public async Task<CallResult<UpdateSubscription>> SubscribeOrderBookRawAsync(string symbol, int? depth, Action<OrderBookRawNotification> action)
		{
			symbol.ValidateLiveCoinSymbol();
			var message = new SubscribeOrderBookChannelRequest
			{
				CurrencyPair = symbol,
				Depth = depth
			};
			var binaryRequest = BuildRequest(message, nameof(SubscribeOrderBookRaw) + NextId(), WsRequestMsgType.SubscribeOrderBookRaw, false);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.OrderBookRawChannelSubscribed;
			binaryRequest.ChannelType = ChannelType.OrderBookRaw;
			binaryRequest.CurrencyPair = symbol;
			binaryRequest.HandleMessageData = HandleOrderBookRawNotificationMessage;
			return await this.Subscribe<OrderBookRawNotification>(_baseAddress, binaryRequest, null,  false, action);
		}
		///<inheritdoc cref="SubscribeOrderBookRawAsync"/>
		public CallResult<UpdateSubscription> SubscribeOrderBookRaw(string symbol, int? depth, Action<OrderBookRawNotification> action) => SubscribeOrderBookRawAsync(symbol, depth, action).Result;

		private static bool HandleOrderBookNotificationMessage(BinaryData request, JToken jtoken, WsResponse header)
		{
			if ((string.IsNullOrWhiteSpace(header?.Meta?.Token) && header?.Meta?.ResponseType == WsResponseMsgType.OrderBookNotify) || (header?.Meta?.ResponseType == WsResponseMsgType.OrderBookChannelSubscribed))
			{
				OrderBookNotification? orderBookNotification = GetWsMessage<OrderBookNotification>(jtoken);
				if (orderBookNotification?.CurrencyPair == request.CurrencyPair)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Subscribe to orderbook notification
		/// </summary>
		/// <param name="symbol">the currency pair</param>
		/// <param name="depth">depth of orderbook, which will be sent in an subscription answer
		///  0 meens 'do not send orderbook snapshot'
		///  if omitted, full orderbook snaphot will be sent
		/// </param>
		/// <param name="action">orderbook notification processing</param>
		/// <returns>subscription result</returns>
		public async Task<CallResult<UpdateSubscription>> SubscribeOrderBookAsync(string symbol, int? depth, Action<OrderBookNotification> action)
		{
			symbol.ValidateLiveCoinSymbol();
			var message = new SubscribeOrderBookChannelRequest
			{
				CurrencyPair = symbol,
				Depth = depth
			};
			var binaryRequest = BuildRequest(message, nameof(SubscribeOrderBook) + NextId(), WsRequestMsgType.SubscribeOrderBook, false);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.OrderBookChannelSubscribed;
			binaryRequest.ChannelType = ChannelType.OrderBook;
			binaryRequest.CurrencyPair = symbol;
			binaryRequest.HandleMessageData = HandleOrderBookNotificationMessage;
			return await this.Subscribe<OrderBookNotification>(_baseAddress, binaryRequest, null, false, action);
		}
		///<inheritdoc cref="SubscribeOrderBookAsync"/>
		public CallResult<UpdateSubscription> SubscribeOrderBook(string symbol, int? depth, Action<OrderBookNotification> action) => SubscribeOrderBookAsync(symbol, depth, action).Result;

		private static bool HandleTradeNotificationMessage(BinaryData request, JToken jtoken, WsResponse header)
		{
			if ((string.IsNullOrWhiteSpace(header?.Meta?.Token) && header?.Meta?.ResponseType == WsResponseMsgType.TradeNotify) || header?.Meta?.ResponseType == WsResponseMsgType.TradeChannelSubscribed)
			{
				TradeNotification? notification = GetWsMessage<TradeNotification>(jtoken);
				if (notification?.CurrencyPair == request.CurrencyPair)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Subscribe to trade notification
		/// </summary>
		/// <param name="symbol">the currency pair</param>
		/// <param name="action">trade notification processing</param>
		/// <returns>subscription result</returns>
		public async Task<CallResult<UpdateSubscription>> SubscribeTradeAsync(string symbol, Action<TradeNotification> action)
		{
			symbol.ValidateLiveCoinSymbol();
			var message = new SubscribeTradeChannelRequest
			{
				CurrencyPair = symbol,
			};
			var binaryRequest = BuildRequest(message, nameof(SubscribeTrade) + NextId(), WsRequestMsgType.SubscribeTrade, false);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.TradeChannelSubscribed;
			binaryRequest.ChannelType = ChannelType.Trade;
			binaryRequest.CurrencyPair = symbol;
			binaryRequest.HandleMessageData = HandleTradeNotificationMessage;
			return await this.Subscribe<TradeNotification>(_baseAddress, binaryRequest, null, false, action);
		}
		///<inheritdoc cref="SubscribeTradeAsync"/>
		public CallResult<UpdateSubscription> SubscribeTrade(string symbol, Action<TradeNotification> action) => SubscribeTradeAsync(symbol, action).Result;

		private static bool HandleCandleNotificationMessage(BinaryData request, JToken jtoken, WsResponse header)
		{
			if ((string.IsNullOrWhiteSpace(header?.Meta?.Token) && header?.Meta?.ResponseType == WsResponseMsgType.CandleNotify) || header?.Meta?.ResponseType == WsResponseMsgType.CandleChannelSubscribed)
			{
				CandleNotification? notification = GetWsMessage<CandleNotification>(jtoken);
				if (notification?.CurrencyPair == request.CurrencyPair)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Subscribe to candles (OHLCV) - public api
		/// </summary>
		/// <param name="symbol">the currency pair</param>
		/// <param name="candleInterval">candle interval</param>
		/// <param name="depth">amount of historical candles to send in answer, 240 is maximum value
		/// default is not to send historical candles at all
		/// </param>
		/// <param name="action">trade notification processing</param>
		/// <returns>subscription result</returns>
		public async Task<CallResult<UpdateSubscription>> SubscribeCandleAsync(string symbol, CandleInterval candleInterval, int? depth, Action<CandleNotification> action)
		{
			symbol.ValidateLiveCoinSymbol();
			var message = new SubscribeCandleChannelRequest
			{
				CurrencyPair = symbol,
				Interval = candleInterval,
				Depth = depth
			};
			var binaryRequest = BuildRequest(message, nameof(SubscribeTrade) + NextId(), WsRequestMsgType.SubscribeCandle, false);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.CandleChannelSubscribed;
			binaryRequest.ChannelType = ChannelType.Candle;
			binaryRequest.CurrencyPair = symbol;
			binaryRequest.HandleMessageData = HandleCandleNotificationMessage;
			return await this.Subscribe<CandleNotification>(_baseAddress, binaryRequest, null,false, action);
		}
		///<inheritdoc cref="SubscribeCandleAsync"/>
		public CallResult<UpdateSubscription> SubscribeCandle(string symbol, CandleInterval candleInterval, int? depth, Action<CandleNotification> action) => SubscribeCandleAsync(symbol, candleInterval, depth, action).Result;

		private static bool HandlePrivateOrderRawNotificationMessage(BinaryData request, JToken jtoken, WsResponse header)
		{
			if ((string.IsNullOrWhiteSpace(header?.Meta?.Token) && header?.Meta?.ResponseType == WsResponseMsgType.PrivateOrderRawNotify) || header?.Meta?.ResponseType == WsResponseMsgType.PrivateOrderRawChannelSubscribed)
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Subscribe to order changes - private api
		/// </summary>
		/// <param name="subscribeType">the subscription type</param>
		/// <param name="action">private order raw notification processing</param>
		/// <returns>subscription result</returns>
		public async Task<CallResult<UpdateSubscription>> SubscribePrivateOrderRawAsync(SubscribeType subscribeType, Action<PrivateOrderRawNotification> action)
		{
			var message = new PrivateSubscribeOrderRawChannelRequest
			{
				SubscribeType = subscribeType,
			};
			var binaryRequest = BuildRequest(message, nameof(SubscribePrivateOrderRaw) + NextId(), WsRequestMsgType.PrivateSubscribeOrderRaw, true);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.PrivateOrderRawChannelSubscribed;
			binaryRequest.PrivateChannelType = PrivateChannelType.OrderRaw;
			binaryRequest.HandleMessageData = HandlePrivateOrderRawNotificationMessage;
			return await this.Subscribe<PrivateOrderRawNotification>(_baseAddress, binaryRequest, null, true, action);
		}
		///<inheritdoc cref="SubscribePrivateOrderRawAsync"/>
		public CallResult<UpdateSubscription> SubscribePrivateOrderRaw(SubscribeType subscribeType, Action<PrivateOrderRawNotification> action) => SubscribePrivateOrderRawAsync(subscribeType, action).Result;

		private static bool HandlePrivateTradeNotificationMessage(BinaryData request, JToken jtoken, WsResponse header)
		{
			if ((string.IsNullOrWhiteSpace(header?.Meta?.Token) && header?.Meta?.ResponseType == WsResponseMsgType.PrivateTradeNotify) || header?.Meta?.ResponseType == WsResponseMsgType.PrivateTradeChannelSubscribed)
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Subscribe to trade changes - private api
		/// </summary>
		/// <param name="action">private trade notification processing</param>
		/// <returns>subscription result</returns>
		public async Task<CallResult<UpdateSubscription>> SubscribePrivateTradeAsync(Action<PrivateTradeNotification> action)
		{
			var message = new PrivateSubscribeTradeChannelRequest
			{
			};
			var binaryRequest = BuildRequest(message, nameof(SubscribePrivateTrade) + NextId(), WsRequestMsgType.PrivateSubscribeTrade, true);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.PrivateTradeChannelSubscribed;
			binaryRequest.PrivateChannelType = PrivateChannelType.Trade;
			binaryRequest.HandleMessageData = HandlePrivateTradeNotificationMessage;
			return await this.Subscribe<PrivateTradeNotification>(_baseAddress, binaryRequest, null, true, action);
		}
		///<inheritdoc cref="SubscribePrivateTradeAsync"/>
		public CallResult<UpdateSubscription> SubscribePrivateTrade(Action<PrivateTradeNotification> action) => SubscribePrivateTradeAsync(action).Result;


		private static bool HandlePrivateChangeBalanceNotificationMessage(BinaryData request, JToken jtoken, WsResponse header)
		{
			if ((string.IsNullOrWhiteSpace(header?.Meta?.Token) && header?.Meta?.ResponseType == WsResponseMsgType.BalanceChangeNotify) || header?.Meta?.ResponseType == WsResponseMsgType.BalanceChangeChannelSubscribed)
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Subscribe to balance changes - private api
		/// </summary>
		/// <param name="action">private balance changes notification processing</param>
		/// <returns>subscription result</returns>
		public async Task<CallResult<UpdateSubscription>> SubscribePrivateChangeBalanceAsync(Action<PrivateChangeBalanceNotification> action)
		{
			var message = new PrivateSubscribeTradeChannelRequest
			{
			};
			var binaryRequest = BuildRequest(message, nameof(SubscribePrivateChangeBalance) + NextId(), WsRequestMsgType.SubscribeBalanceChange, true);
			binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.BalanceChangeChannelSubscribed;
			binaryRequest.PrivateChannelType = PrivateChannelType.ChangeBalance;
			binaryRequest.HandleMessageData = HandlePrivateChangeBalanceNotificationMessage;
			return await this.Subscribe<PrivateChangeBalanceNotification>(_baseAddress, binaryRequest, null, true, action);
		}
		///<inheritdoc cref="SubscribePrivateChangeBalanceAsync"/>
		public CallResult<UpdateSubscription> SubscribePrivateChangeBalance(Action<PrivateChangeBalanceNotification> action) => SubscribePrivateChangeBalanceAsync(action).Result;


		/// <inheritdoc/>
		protected override async Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
		{
			if (authProvider == null)
				return new CallResult<bool>(false, new NoApiCredentialsError());
			var message = new LoginRequest
			{
				ApiKey = authProvider.Credentials?.Key?.GetString(),
			};
			var binaryRequest = BuildRequest(message, nameof(AuthenticateSocket) + NextId(), WsRequestMsgType.Login, true);
			var result = new CallResult<bool>(false, new ServerError("No response from server"));
			await s.SendAndWait(binaryRequest, ResponseTimeout, data =>
			{
				var response = GetWsResponse(data);
				if (response?.Meta?.ResponseType == WsResponseMsgType.LoginResponse && response.Meta.Token == binaryRequest.Token)
				{
					log.Write(LogVerbosity.Debug, "Authorization completed");
					result = new CallResult<bool>(true, null);
					return true;
				}
				if (response?.Meta?.ResponseType == WsResponseMsgType.Error && response.Meta.Token == binaryRequest.Token)
				{
					var error = GetWsMessage<ErrorResponse>(data);
					log.Write(LogVerbosity.Debug, $"Authorization failed {error?.Code} {error?.Message}");
					result = new CallResult<bool>(false, new ServerError(error?.Code ?? -1, error?.Message ?? "Unknwon server error"));
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
			if (response?.Meta?.Token == fullRequest.Token)
			{
				if (response?.Meta?.ResponseType == fullRequest.ExpectedResponseMsgType)
				{
					callResult = new CallResult<T>(GetWsMessage<T>(data), null);
					log.Write(LogVerbosity.Debug, $"{nameof(HandleQueryResponse)} match found in message:{data}, request:{request}, response:{response}");
					return true;
				}
				else if (response?.Meta?.ResponseType == WsResponseMsgType.Error)
				{
					var error = GetWsMessage<ErrorResponse>(data);
					if (error != null)
					{
						callResult = new CallResult<T>(default(T), new ServerError(error.Code, error.Message ?? "An error occured"));
						log.Write(LogVerbosity.Debug, $"{nameof(HandleQueryResponse)} error found in message:{data}, request:{request}, response:{response}");
						return true;
					}
				}
				callResult = new CallResult<T>(default(T), new ServerError(-1, $"Unknown message type received {response?.Meta?.ResponseType}"));
				log.Write(LogVerbosity.Debug, $"{nameof(HandleQueryResponse)} unknown message in message:{data}, request:{request}, response:{response}");
				return true;

			}
			if (response?.Meta?.ResponseType == WsResponseMsgType.PrivateChannelUnsubscribed && fullRequest.ExpectedResponseMsgType == WsResponseMsgType.PrivateChannelUnsubscribed)
			{
				var privateUnsubscribeResponse = GetWsMessage<PrivateChannelUnsubscribedResponse>(data);
				if (privateUnsubscribeResponse.PrivateChannelType == fullRequest.PrivateChannelType)
				{
					callResult = new CallResult<T>(GetWsMessage<T>(data), null);
					log.Write(LogVerbosity.Debug, $"{nameof(HandleQueryResponse)} private match found in message:{data}, request:{request}, response:{response}");
					return true;
				}
			}
			callResult = new CallResult<T>(default(T), null);
			return false;
		}
		static readonly ConditionalWeakTable<JToken, WsResponse> ExtendedWsResponse = new ConditionalWeakTable<JToken, WsResponse>();
		private static WsResponse ExtendedWsResponseCreateValueFunction(JToken key)
		{
			var strData = key.Value<string>("Data");
			using (MemoryStream responseStream = new MemoryStream(System.Convert.FromBase64String(strData)))
			{
				WsResponse response = ProtoBuf.Serializer.Deserialize<WsResponse>(responseStream);
				return response;
			};
		}
		static readonly ConditionalWeakTable<JToken, Object?> ExtendedMessage = new ConditionalWeakTable<JToken, object?>();
		private static T? DecodeMessage<T>(byte[]? msg) where T : class
		{
			if (msg == null || msg.Length == 0)
				return null;
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
			switch (response?.Meta?.ResponseType)
			{
				case WsResponseMsgType.TickerChannelSubscribed:
					return DecodeMessage<TickerNotification>(msg);
				case WsResponseMsgType.OrderBookRawChannelSubscribed:
					return DecodeMessage<OrderBookRawNotification>(msg);
				case WsResponseMsgType.OrderBookChannelSubscribed:
					return DecodeMessage<OrderBookNotification>(msg);
				case WsResponseMsgType.TradeChannelSubscribed:
					return DecodeMessage<TradeNotification>(msg);
				case WsResponseMsgType.CandleChannelSubscribed:
					return DecodeMessage<CandleNotification>(msg);
				case WsResponseMsgType.ChannelUnsubscribed:
					return DecodeMessage<ChannelUnsubscribedResponse>(msg);
				case WsResponseMsgType.Error:
					return DecodeMessage<ErrorResponse>(msg);
				case WsResponseMsgType.TickerNotify:
					return DecodeMessage<TickerNotification>(msg);
				case WsResponseMsgType.OrderBookRawNotify:
					return DecodeMessage<OrderBookRawNotification>(msg);
				case WsResponseMsgType.OrderBookNotify:
					return DecodeMessage<OrderBookNotification>(msg);
				case WsResponseMsgType.TradeNotify:
					return DecodeMessage<TradeNotification>(msg);
				case WsResponseMsgType.CandleNotify:
					return DecodeMessage<CandleNotification>(msg);
				case WsResponseMsgType.LoginResponse:
					return null;
				case WsResponseMsgType.PutLimitOrderResponse:
					return DecodeMessage<PutLimitOrderResponse>(msg);
				case WsResponseMsgType.CancelLimitOrderResponse:
					return DecodeMessage<CancelLimitOrderResponse>(msg);
				case WsResponseMsgType.BalanceResponse:
					break;
				case WsResponseMsgType.BalancesResponse:
					break;
				case WsResponseMsgType.LastTradesResponse:
					break;
				case WsResponseMsgType.TradesResponse:
					break;
				case WsResponseMsgType.ClientOrdersResponse:
					return DecodeMessage<ClientOrdersResponse>(msg);
				case WsResponseMsgType.ClientOrderResponse:
					return DecodeMessage<ClientOrderResponse>(msg);
				case WsResponseMsgType.CommissionResponse:
					break;
				case WsResponseMsgType.CommissionCommonInfoResponse:
					break;
				case WsResponseMsgType.TradeHistoryResponse:
					break;
				case WsResponseMsgType.MarketOrderResponse:
					break;
				case WsResponseMsgType.WalletAddressResponse:
					break;
				case WsResponseMsgType.WithdrawalCoinResponse:
					break;
				case WsResponseMsgType.WithdrawalPayeerResponse:
					break;
				case WsResponseMsgType.WithdrawalCapitalistResponse:
					break;
				case WsResponseMsgType.WithdrawalAdvcashResponse:
					break;
				case WsResponseMsgType.PrivateOrderRawChannelSubscribed:
					return DecodeMessage<PrivateOrderRawNotification>(msg);
				case WsResponseMsgType.PrivateTradeChannelSubscribed:
					return DecodeMessage<PrivateTradeNotification>(msg);
				case WsResponseMsgType.PrivateOrderRawNotify:
					return DecodeMessage<PrivateOrderRawNotification>(msg);
				case WsResponseMsgType.PrivateTradeNotify:
					return DecodeMessage<PrivateTradeNotification>(msg);
				case WsResponseMsgType.PrivateChannelUnsubscribed:
					return DecodeMessage<PrivateChannelUnsubscribedResponse>(msg);
				case WsResponseMsgType.WithdrawalYandexResponse:
					break;
				case WsResponseMsgType.WithdrawalQiwiResponse:
					break;
				case WsResponseMsgType.WithdrawalCardResponse:
					break;
				case WsResponseMsgType.WithdrawalMastercardResponse:
					break;
				case WsResponseMsgType.WithdrawalPerfectmoneyResponse:
					break;
				case WsResponseMsgType.VoucherMakeResponse:
					break;
				case WsResponseMsgType.VoucherAmountResponse:
					break;
				case WsResponseMsgType.VoucherRedeemResponse:
					break;
				case WsResponseMsgType.CancelOrdersResponse:
					return DecodeMessage<CancelOrdersResponse>(msg);
				case WsResponseMsgType.PongResponse:
					return DecodeMessage<PongResponse>(msg);
				case WsResponseMsgType.BalanceChangeChannelSubscribed:
					return DecodeMessage<PrivateChangeBalanceNotification>(msg);
				case WsResponseMsgType.BalanceChangeNotify:
					return DecodeMessage<PrivateChangeBalanceNotification>(msg);
				default:
					break;
			}
			throw new System.ArgumentException($"Unknown message type {response?.Meta?.ResponseType}");
		}
		private static WsResponse GetWsResponse(JToken key)
		{
			return ExtendedWsResponse.GetValue(key, ExtendedWsResponseCreateValueFunction);
		}
		private static T GetWsMessage<T>(JToken key)  //where T : class
		{
			T result = default(T);
			object? r = ExtendedMessage.GetValue(key, ExtendedMessageCreateValueFunction);
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
			if (response?.Meta?.Token == ((BinaryData)request).Token)
			{
				if (response?.Meta?.ResponseType == fullRequest.ExpectedResponseMsgType)
				{
					callResult = new CallResult<object>(true, null);
					log.Write(LogVerbosity.Debug, $"{nameof(HandleSubscriptionResponse)} match between message:{jmessage}, request:{request}, response:{response}");
					return true;
				}
				else if (response?.Meta?.ResponseType == WsResponseMsgType.Error)
				{
					var error = GetWsMessage<ErrorResponse>(jmessage);
					if (error != null)
					{
						callResult = new CallResult<object>(false, new ServerError(error.Code, error.Message ?? "An error occured"));
						log.Write(LogVerbosity.Debug, $"{nameof(HandleSubscriptionResponse)} error found in message:{jmessage}, request:{request}, response:{response}");
						return true;
					}
				}
				callResult = new CallResult<object>(false, new ServerError(-1, $"Unknown message type received {response?.Meta?.ResponseType}"));
				log.Write(LogVerbosity.Debug, $"{nameof(HandleSubscriptionResponse)} unknown message in message:{jmessage}, request:{request}, response:{response}");
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
			if (!string.IsNullOrEmpty(response?.Meta?.Token))
			{
				return false;
			}
			else if (response == null)
			{
				return false;
			}
			else
			{
				var match = fullRequest.HandleMessageData?.Invoke(fullRequest, message, response) == true;
				if (match)
				{
					log.Write(LogVerbosity.Debug, $"{nameof(MessageMatchesHandler)} match between message:{message}, request:{request}, response:{response}");
				};
				return match;
			};
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
			if (fullRequest.ChannelType != null && fullRequest?.CurrencyPair == null)
			{
				return false;
			}
			if (fullRequest.ChannelType == null && fullRequest.PrivateChannelType == null)
			{
				return false;
			}
			if (fullRequest.ChannelType != null)
			{
				var request = new UnsubscribeRequest
				{
					ChannelType = fullRequest.ChannelType ?? ((ChannelType)(-1)),
					CurrencyPair = fullRequest.CurrencyPair
				};
				var binaryRequest = BuildRequest(request, nameof(Unsubscribe) + NextId().ToString(), WsRequestMsgType.Unsubscribe, false);
				binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.ChannelUnsubscribed;
				var res = await QueryAndWait<ChannelUnsubscribedResponse>(connection, binaryRequest);
				return res.Success;
			}
			else
			{
				var request = new PrivateUnsubscribeRequest
				{
					PrivateChannelType = fullRequest.PrivateChannelType ?? ((PrivateChannelType)(-1)),
				};
				var binaryRequest = BuildRequest(request, nameof(Unsubscribe) + NextId().ToString(), WsRequestMsgType.PrivateUnsubscribe, true);
				binaryRequest.ExpectedResponseMsgType = WsResponseMsgType.PrivateChannelUnsubscribed;
				binaryRequest.PrivateChannelType = request.PrivateChannelType;
				var res = await QueryAndWait<PrivateChannelUnsubscribedResponse>(connection, binaryRequest);
				return res.Success;
			}
		}
		/// <inheritdoc/>
		protected override void HandleUnhandledMessage(JToken token)
		{
			var m = GetWsResponse(token);
			log.Write(LogVerbosity.Error, $"Unhandled message. ResponseType:{m?.Meta?.ResponseType}, Token:{m?.Meta?.Token} Msg.Length:{m?.Msg?.Length}");
			base.HandleUnhandledMessage(token);
		}
		#endregion
	}
}
