using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using LiveCoin.Net.Converters;
using LiveCoin.Net.Enums;
using LiveCoin.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LiveCoin.Net
{
	/// <summary>
	/// Client providing access to the LiveCoin REST Api
	/// </summary>
	public class LiveCoinClient : RestClient
	{
		#region fields 
		private static LiveCoinClientOptions defaultOptions = new LiveCoinClientOptions();
		private static LiveCoinClientOptions DefaultOptions => defaultOptions.Copy();

		// Public
		private const string Price24HEndpoint = "exchange/ticker";
		private const string CoinsInfoEndpoint = "info/coinInfo";
		private const string OrderBookEndpoint = "exchange/order_book";
		private const string AllOrderBookEndpoint = "exchange/all/order_book";
		private const string TradesHistoryEndpoint = "exchange/last_trades";
		private const string ExchangeRestrictionsEndpoint = "exchange/restrictions";
		private const string ExchangeMaxBidMinAskEndpoint = "exchange/maxbid_minask";


		// Private
		private const string ExchangeTradesEndpoint = "exchange/trades";
		private const string PaymentBalancesEndpoint = "payment/balances";
		private const string PaymentBalanceEndpoint = "payment/balance";
		private const string PaymentHistoryTransactionEndpoint = "payment/history/transactions";
		private const string PaymentHistoryTransactionSiezEndpoint = "payment/history/size";
		private const string ExchangeBuyLimitEndpoint = "exchange/buylimit";
		private const string ExchangeSellLimitEndpoint = "exchange/selllimit";
		private const string ExchangeBuyMarketEndpoint = "exchange/buymarket";
		private const string ExchangeSellMarketEndpoint = "exchange/sellmarket";
		private const string ExchangeCancelLimitEndpoint = "exchange/cancellimit";
		private const string ExchangeClientOrdersEndpoint = "exchange/client_orders";
		private const string ExchangeCommissionEndpoint = "exchange/commission";
		private const string ExchangeCommissionCommonInfoEndpoint = "exchange/commissionCommonInfo";
		private const string ExchangeOrderEndpoint = "exchange/order";
		private const string PaymentAddressEndPoint = "payment/get/address";


		#endregion

		#region constructor/destructor
		/// <summary>
		/// Create a new instance of LiveCoinClient using the default options
		/// </summary>
		public LiveCoinClient() : this(DefaultOptions)
		{
		}

		/// <summary>
		/// Create a new instance of LiveCoinClient using provided options
		/// </summary>
		/// <param name="options">The options to use for this client</param>
		public LiveCoinClient(LiveCoinClientOptions options) : base("LiveCoin", options, options.ApiCredentials == null ? null : new LiveCoinAuthenticationProvider(options.ApiCredentials))
		{
			manualParseError = true;
			arraySerialization = ArrayParametersSerialization.MultipleValues;
			requestBodyFormat = RequestBodyFormat.FormData;
		}
		#endregion

		#region methods
		#region public data

		/// <summary>
		/// Set the default options to be used when creating new clients
		/// </summary>
		/// <param name="options"></param>
		public static void SetDefaultOptions(LiveCoinClientOptions options)
		{
			defaultOptions = options;
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

		/// <summary>
		/// Get information on specified currency pair for the last 24 hours.
		/// </summary>
		/// <param name="symbol">Identifier of currency pair.</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Information on specified currency pair</returns>
		public async Task<WebCallResult<LiveCoin24HTicker>> Get24HTickerAsync(string symbol, CancellationToken ct = default)
		{
			symbol.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object> { { "currencyPair", symbol } };
			return await SendRequest<LiveCoin24HTicker>(GetUrl(Price24HEndpoint), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
		}
		/// <summary>
		/// Get information on specified currency pair for the last 24 hours.
		/// </summary>
		/// <param name="symbol">Identifier of currency pair</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Information on specified currency pair</returns>
		public WebCallResult<LiveCoin24HTicker> Get24HTicker(string symbol, CancellationToken ct = default) => Get24HTickerAsync(symbol, ct).Result;


		/// <summary>
		/// Get information on currency pairs for the last 24 hours.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Information on specified currency pair</returns>
		public async Task<WebCallResult<LiveCoin24HTicker[]>> Get24HTickersAsync(CancellationToken ct = default)
		{
			return await SendRequest<LiveCoin24HTicker[]>(GetUrl(Price24HEndpoint), HttpMethod.Get, ct).ConfigureAwait(false);
		}
		/// <summary>
		/// Get information on currency pairs for the last 24 hours.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Information on specified currency pair</returns>
		public WebCallResult<LiveCoin24HTicker[]> Get24HTickers(CancellationToken ct = default) => Get24HTickersAsync(ct).Result;

		/// <summary>
		/// Get a detailed review on the latest transactions for requested currency pair. You may receive the update for the last hour or for the last minute.
		/// </summary>
		/// <param name="symbol">Identifier of currency pair.</param>
		/// <param name="onlyForLastMinute">Return information for the last minute if true. Return information for the last hour if false.</param>
		/// <param name="onlySide">Return information for only one side.</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Information on all currency pairs</returns>
		public async Task<WebCallResult<LiveCoinTrade[]>> GetTradesHistoryAsync(string symbol, bool? onlyForLastMinute = null, Side? onlySide = null, CancellationToken ct = default)
		{
			symbol.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object> { { "currencyPair", symbol } };
			parameters.AddOptionalParameter("minutesOrHour", onlyForLastMinute?.ToString(System.Globalization.CultureInfo.InvariantCulture));
			parameters.AddOptionalParameter("type", onlySide == null ? null : JsonConvert.SerializeObject(onlySide, new SideConverter(false)));
			return await SendRequest<LiveCoinTrade[]>(GetUrl(TradesHistoryEndpoint), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
		}
		/// <summary>
		/// Get a detailed review on the latest transactions for requested currency pair. You may receive the update for the last hour or for the last minute.
		/// </summary>
		/// <param name="symbol">Identifier of currency pair.</param>
		/// <param name="onlyForLastMinute">Return information for the last minute if true. Return information for the last hour if false.</param>
		/// <param name="onlySide">Return information for only one side.</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Information on all currency pairs</returns>
		public WebCallResult<LiveCoinTrade[]> GetTradesHistory(string symbol, bool? onlyForLastMinute = null, Side? onlySide = null, CancellationToken ct = default) => GetTradesHistoryAsync(symbol, onlyForLastMinute, onlySide, ct).Result;

		/// <summary>
		/// Get the orderbook for specified currency pair (you may enable the feature of grouping orders by price).
		/// </summary>
		/// <param name="symbol">Identifier of currency pair.</param>
		/// <param name="groupByPrice">If true, orders will be grouped by price</param>
		/// <param name="depth">Returns maximum amount of bids (asks).</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>The order book</returns>
		public async Task<WebCallResult<LiveCoinOrderBook>> GetOrderBookAsync(string symbol, bool? groupByPrice = null, int? depth = null, CancellationToken ct = default)
		{
			symbol.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object> { { "currencyPair", symbol } };
			parameters.AddOptionalParameter("groupByPrice", groupByPrice?.ToString(System.Globalization.CultureInfo.InvariantCulture));
			parameters.AddOptionalParameter("depth", depth);
			return await SendRequest<LiveCoinOrderBook>(GetUrl(OrderBookEndpoint), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
		}
		/// <summary>
		/// Get the orderbook for specified currency pair (you may enable the feature of grouping orders by price).
		/// </summary>
		/// <param name="symbol">Identifier of currency pair.</param>
		/// <param name="groupByPrice">If true, orders will be grouped by price</param>
		/// <param name="depth">Returns maximum amount of bids (asks).</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>The order book</returns>
		public WebCallResult<LiveCoinOrderBook> GetOrderBook(string symbol, bool? groupByPrice = null, int? depth = null, CancellationToken ct = default) => GetOrderBookAsync(symbol, groupByPrice, depth, ct).Result;

		/// <summary>
		/// Get the orderbook for all currency pairs (you may enable the feature of grouping orders by price).
		/// </summary>
		/// <param name="groupByPrice">If true, orders will be grouped by price</param>
		/// <param name="depth">Returns maximum amount of bids (asks).</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>A dictionary of all order books by symbol</returns>
		public async Task<WebCallResult<Dictionary<string, LiveCoinOrderBook>>> GetAllOrderBookAsync(bool? groupByPrice = null, int? depth = null, CancellationToken ct = default)
		{
			var parameters = new Dictionary<string, object>();
			parameters.AddOptionalParameter("groupByPrice", groupByPrice?.ToString(System.Globalization.CultureInfo.InvariantCulture));
			parameters.AddOptionalParameter("depth", depth);
			return await SendRequest<Dictionary<string, LiveCoinOrderBook>>(GetUrl(AllOrderBookEndpoint), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
		}
		/// <summary>
		/// Get the orderbook for all currency pairs (you may enable the feature of grouping orders by price).
		/// </summary>
		/// <param name="groupByPrice">If true, orders will be grouped by price</param>
		/// <param name="depth">Returns maximum amount of bids (asks).</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>A dictionary of all order books by symbol</returns>
		public WebCallResult<Dictionary<string, LiveCoinOrderBook>> GetAllOrderBook(bool? groupByPrice = null, int? depth = null, CancellationToken ct = default) => GetAllOrderBookAsync(groupByPrice, depth, ct).Result;

		/// <summary>
		/// Returns maximum bid and minimum ask in the current orderbook.
		/// Without specifying a currency pair, all pairs are returned.
		/// </summary>
		/// <param name="symbol">Identifier of currency pair.</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Maximum bid and minimum ask in the orderbook.</returns>
		public async Task<WebCallResult<LiveCoinMaxBidMinAsk>> GetMaxBidAndMinAskAsync(string? symbol = null, CancellationToken ct = default)
		{
			symbol?.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object>();
			parameters.AddOptionalParameter("currencyPair", symbol);
			return await SendRequest<LiveCoinMaxBidMinAsk>(GetUrl(ExchangeMaxBidMinAskEndpoint), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
		}
		/// <summary>
		/// Returns maximum bid and minimum ask in the current orderbook.
		/// Without specifying a currency pair, all pairs are returned.
		/// </summary>
		/// <param name="symbol">Identifier of currency pair.</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Maximum bid and minimum ask in the orderbook.</returns>
		public WebCallResult<LiveCoinMaxBidMinAsk> GetMaxBidAndMinAsk(string? symbol = null, CancellationToken ct = default) => GetMaxBidAndMinAskAsync(symbol, ct).Result;

		/// <summary>
		/// Returns the limit for minimum amount to open order, for each pair. Also returns maximum number of digits after the decimal point in price value.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>The limit for minimum amount to open order, for each pair. Also the maximum number of digits after the decimal point in price value.</returns>
		public async Task<WebCallResult<LiveCoinExchangeRestrictions>> GetExchangeRestrictionsAsync(CancellationToken ct = default)
		{
			return await SendRequest<LiveCoinExchangeRestrictions>(GetUrl(ExchangeRestrictionsEndpoint), HttpMethod.Get, ct).ConfigureAwait(false);
		}
		/// <summary>
		/// Returns the limit for minimum amount to open order, for each pair. Also returns maximum number of digits after the decimal point in price value.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>The limit for minimum amount to open order, for each pair. Also the maximum number of digits after the decimal point in price value.</returns>
		public WebCallResult<LiveCoinExchangeRestrictions> GetExchangeRestrictions(CancellationToken ct = default) => GetExchangeRestrictionsAsync(ct).Result;

		/// <summary>
		/// Returns public data for currencies.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Public data for currencies.</returns>
		public async Task<WebCallResult<LiveCoinCoinInfo>> GetCoinsInfoAsync(CancellationToken ct = default)
		{
			return await SendRequest<LiveCoinCoinInfo>(GetUrl(CoinsInfoEndpoint), HttpMethod.Get, ct).ConfigureAwait(false);
		}
		/// <summary>
		/// Returns public data for currencies.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Public data for currencies.</returns>
		public WebCallResult<LiveCoinCoinInfo> GetCoinsInfo(CancellationToken ct = default) => GetCoinsInfoAsync(ct).Result;

		#endregion
		#region customer private data
		/// <summary>
		/// Get information on your latest transactions. The return may be limited by the parameters below.
		/// </summary>
		/// <param name="symbol">Identifier of currency pair. If null, all pairs will be returned.</param>
		/// <param name="newOrdersFirst">Sorting order. If true then new orders will be first, otherwise old orders will be first.</param>
		/// <param name="limit">Number of items per page</param>
		/// <param name="pageOffset">Page offset (position of the first item on the page)</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Latest transactions</returns>
		public async Task<WebCallResult<LiveCoinClientTrade[]>> GetClientTradesAsync(string? symbol = null, bool? newOrdersFirst = null, int? limit = null, int? pageOffset = null, CancellationToken ct = default)
		{
			symbol?.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object>();
			parameters.AddOptionalParameter("currencyPair", symbol);
			parameters.AddOptionalParameter("orderDesc", newOrdersFirst?.ToString(System.Globalization.CultureInfo.InvariantCulture));
			parameters.AddOptionalParameter("limit", limit);
			parameters.AddOptionalParameter("offset", pageOffset);
			return await SendRequest<LiveCoinClientTrade[]>(GetUrl(ExchangeTradesEndpoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Get information on your latest transactions. The return may be limited by the parameters below.
		/// </summary>
		/// <param name="symbol">Identifier of currency pair. If null, all pairs will be returned.</param>
		/// <param name="newOrdersFirst">Sorting order. If true then new orders will be first, otherwise old orders will be first.</param>
		/// <param name="limit">Number of items per page</param>
		/// <param name="pageOffset">Page offset (position of the first item on the page)</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Latest transactions</returns>
		public WebCallResult<LiveCoinClientTrade[]> GetClientTrades(string? symbol = null, bool? newOrdersFirst = null, int? limit = null, int? pageOffset = null, CancellationToken ct = default) => GetClientTradesAsync(symbol, newOrdersFirst, limit, pageOffset, ct).Result;

		/// <summary>
		/// Get a detailed review of your orders for requested currency pair. You can optionally limit return of orders of a certain type (return only open or only closed orders)
		/// </summary>
		/// <param name="symbol">Identifier of currency pair. If undefined, all pairs will be returned.</param>
		/// <param name="openClosed">Order type filter. Default value: All</param>
		/// <param name="issuedFrom">Start date</param>
		/// <param name="issuedTo">End date</param>
		/// <param name="startRow">Sequence number of the first record</param>
		/// <param name="endRow">Sequence number of the last record</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Detailed review of your orders</returns>
		public async Task<WebCallResult<LiveCoinClientOrders>> GetClientOrdersAsync(string? symbol = null, OrderStatusFilter? openClosed = null, DateTime? issuedFrom = null, DateTime? issuedTo = null, long? startRow = null, long? endRow = null, CancellationToken ct = default)
		{
			symbol?.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object>();
			parameters.AddOptionalParameter("currencyPair", symbol);
			parameters.AddOptionalParameter("openClosed", openClosed == null ? null : JsonConvert.SerializeObject(openClosed, new OrderStatusFilterConverter(false)));
			parameters.AddOptionalParameter("issuedFrom", issuedFrom?.ToUnixMilliseconds());
			parameters.AddOptionalParameter("issuedTo", issuedTo?.ToUnixMilliseconds());
			parameters.AddOptionalParameter("startRow", startRow);
			parameters.AddOptionalParameter("endRow", endRow);
			return await SendRequest<LiveCoinClientOrders>(GetUrl(ExchangeClientOrdersEndpoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Get a detailed review of your orders for requested currency pair. You can optionally limit return of orders of a certain type (return only open or only closed orders)
		/// </summary>
		/// <param name="symbol">Identifier of currency pair. If undefined, all pairs will be returned.</param>
		/// <param name="openClosed">Order type filter. Default value: All</param>
		/// <param name="issuedFrom">Start date</param>
		/// <param name="issuedTo">End date</param>
		/// <param name="startRow">Sequence number of the first record</param>
		/// <param name="endRow">Sequence number of the last record</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>Detailed review of your orders</returns>
		public WebCallResult<LiveCoinClientOrders> GetClientOrders(string? symbol = null, OrderStatusFilter? openClosed = null, DateTime? issuedFrom = null, DateTime? issuedTo = null, long? startRow = null, long? endRow = null, CancellationToken ct = default) => GetClientOrdersAsync(symbol, openClosed, issuedFrom, issuedTo, startRow, endRow, ct).Result;
		/// <summary>
		/// Get the order information by its ID
		/// </summary>
		/// <param name="orderId">Order ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order information</returns>
		public async Task<WebCallResult<LiveCoinExchangeOrder>> GetClientOrderAsync(long orderId, CancellationToken ct = default)
		{
			var parameters = new Dictionary<string, object> { { "orderId", orderId } };
			return await SendRequest<LiveCoinExchangeOrder>(GetUrl(ExchangeOrderEndpoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Get the order information by its ID
		/// </summary>
		/// <param name="orderId">Order ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order information</returns>
		public WebCallResult<LiveCoinExchangeOrder> GetClientOrder(long orderId, CancellationToken ct = default) => GetClientOrderAsync(orderId, ct).Result;


		/// <summary>
		/// Returns an array of your balances. There are four types of balances for every currency: 
		/// total : total, 
		/// available : funds available for trading,
		/// trade : funds in open orders,
		/// availableWithdrawal : funds available for withdraw
		/// </summary>
		/// <param name="currencies">list of currency (currencies). If not specified, response will return balances for all currencies</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>array of your balances</returns>
		public async Task<WebCallResult<LiveCoinBalanceElement[]>> GetBalancesAsync(IEnumerable<string>? currencies = null, CancellationToken ct = default)
		{
			var parameters = new Dictionary<string, object>();
			if (currencies?.Any() == true)
			{
				parameters.AddOptionalParameter("currency", string.Join(",", currencies));
			}
			return await SendRequest<LiveCoinBalanceElement[]>(GetUrl(PaymentBalancesEndpoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Returns an array of your balances. There are four types of balances for every currency: 
		/// total : total, 
		/// available : funds available for trading,
		/// trade : funds in open orders,
		/// availableWithdrawal : funds available for withdraw
		/// </summary>
		/// <param name="currencies">list of currency (currencies). If not specified, response will return balances for all currencies</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>array of your balances</returns>
		public WebCallResult<LiveCoinBalanceElement[]> GetBalances(IEnumerable<string>? currencies = null, CancellationToken ct = default) => GetBalancesAsync(currencies, ct).Result;

		/// <summary>
		/// Returns the funds available for trading
		/// </summary>
		/// <param name="currency">currency</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>your balance</returns>
		public async Task<WebCallResult<LiveCoinBalanceElement>> GetAvailableBalanceAsync(string currency, CancellationToken ct = default)
		{
			var parameters = new Dictionary<string, object>() { { "currency", currency } };
			return await SendRequest<LiveCoinBalanceElement>(GetUrl(PaymentBalanceEndpoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Returns the funds available for trading
		/// </summary>
		/// <param name="currency">currency</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>your balance</returns>
		public WebCallResult<LiveCoinBalanceElement> GetAvailableBalance(string currency, CancellationToken ct = default) => GetAvailableBalanceAsync(currency, ct).Result;

		/// <summary>
		/// Returns a list of your transactions
		/// </summary>
		/// <param name="startDate">Start date</param>
		/// <param name="endDate">End date</param>
		/// <param name="transactionTypes">Transaction types</param>
		/// <param name="limit">Maximum number of results</param>
		/// <param name="offset">First index</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>list of your transactions</returns>
		public async Task<WebCallResult<LiveCoinTransaction[]>> GetPaymentHistoryAsync(DateTime startDate, DateTime endDate, IEnumerable<TransactionType>? transactionTypes = null, int? limit = null, int? offset = null, CancellationToken ct = default)
		{
			var parameters = new Dictionary<string, object>() {
				{ "start", startDate.ToUnixMilliseconds()},
				{ "end", endDate.ToUnixMilliseconds()},
			};
			if (transactionTypes?.Any() == true)
			{
				parameters.Add("types", string.Join(",", transactionTypes.Select(tt => JsonConvert.SerializeObject(tt, new TransactionTypeConverter(false)))));
			}
			parameters.AddOptionalParameter("limit", limit);
			parameters.AddOptionalParameter("offset", offset);
			return await SendRequest<LiveCoinTransaction[]>(GetUrl(PaymentHistoryTransactionEndpoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Returns a list of your transactions
		/// </summary>
		/// <param name="startDate">Start date</param>
		/// <param name="endDate">End date</param>
		/// <param name="transactionTypes">Transaction types</param>
		/// <param name="limit">Maximum number of results</param>
		/// <param name="offset">First index</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>list of your transactions</returns>
		public WebCallResult<LiveCoinTransaction[]> GetPaymentHistory(DateTime startDate, DateTime endDate, IEnumerable<TransactionType>? transactionTypes = null, int? limit = null, int? offset = null, CancellationToken ct = default) => GetPaymentHistoryAsync(startDate, endDate, transactionTypes, limit, offset, ct).Result;

		/// <summary>
		/// Returns the number of your transactions
		/// </summary>
		/// <param name="startDate">Start date</param>
		/// <param name="endDate">End date</param>
		/// <param name="transactionTypes">Transaction types</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>number of transactions</returns>
		public async Task<WebCallResult<int>> GetPaymentHistorySizeAsync(DateTime startDate, DateTime endDate, IEnumerable<TransactionType>? transactionTypes = null, CancellationToken ct = default)
		{
			var parameters = new Dictionary<string, object>() {
				{ "start", startDate.ToUnixMilliseconds()},
				{ "end", endDate.ToUnixMilliseconds()},
			};
			if (transactionTypes?.Any() == true)
			{
				parameters.Add("types", string.Join(",", transactionTypes.Select(tt => JsonConvert.SerializeObject(tt, new TransactionTypeConverter(false)))));
			}
			var stringResult = await SendRequest<string>(GetUrl(PaymentHistoryTransactionSiezEndpoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
			var result = new WebCallResult<int>(stringResult.ResponseStatusCode, stringResult.ResponseHeaders, 0, stringResult.Error);
			int size = 0;
			if (stringResult.Success)
			{
				if (!int.TryParse(stringResult.Data, out size))
				{
					return new WebCallResult<int>(
						stringResult.ResponseStatusCode,
						stringResult.ResponseHeaders,
						0,
						new DeserializeError($"Cannot deserialize size", stringResult.Data)
						);
				}
			}
			return new WebCallResult<int>(stringResult.ResponseStatusCode, stringResult.ResponseHeaders, size, stringResult.Error);
		}

		/// <summary>
		/// Returns the number of your transactions
		/// </summary>
		/// <param name="startDate">Start date</param>
		/// <param name="endDate">End date</param>
		/// <param name="transactionTypes">Transaction types</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>number of transactions</returns>
		public WebCallResult<int> GetPaymentHistorySize(DateTime startDate, DateTime endDate, IEnumerable<TransactionType>? transactionTypes = null, CancellationToken ct = default) => GetPaymentHistorySizeAsync(startDate, endDate, transactionTypes, ct).Result;

		/// <summary>
		/// Returns actual trading fee for customer
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>actual trading fee for customer</returns>
		public async Task<WebCallResult<LiveCoinCommissionResult>> GetCommissionAsync(CancellationToken ct = default)
		{
			return await SendRequest<LiveCoinCommissionResult>(GetUrl(ExchangeCommissionEndpoint), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Returns actual trading fee for customer
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>actual trading fee for customer</returns>
		public WebCallResult<LiveCoinCommissionResult> GetCommission(CancellationToken ct = default) => GetCommissionAsync(ct).Result;

		/// <summary>
		/// Returns actual trading fee and volume for the last 30 days in USD
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>actual trading fee and volume for the last 30 days in USD for customer</returns>
		public async Task<WebCallResult<LiveCoinCommissionInfo>> GetCommissionCommonInfoAsync(CancellationToken ct = default)
		{
			return await SendRequest<LiveCoinCommissionInfo>(GetUrl(ExchangeCommissionCommonInfoEndpoint), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Returns actual trading fee and volume for the last 30 days in USD
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <returns>actual trading fee and volume for the last 30 days in USD for customer</returns>
		public WebCallResult<LiveCoinCommissionInfo> GetCommissionCommonInfo(CancellationToken ct = default) => GetCommissionCommonInfoAsync(ct).Result;
		#endregion
		#region open/cancel order
		/// <summary>
		/// Open a buy order (limit) for particular currency pair.
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="price">price</param>
		/// <param name="quantity">quantity</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order result with order id</returns>
		public async Task<WebCallResult<LiveCoinOrderResult>> BuyLimitOrderAsync(string symbol, decimal price, decimal quantity, CancellationToken ct = default)
		{
			symbol.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object> { { "currencyPair", symbol },
				{"price", price },
				{"quantity", quantity } };
			return await SendRequest<LiveCoinOrderResult>(GetUrl(ExchangeBuyLimitEndpoint), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Open a buy order (limit) for particular currency pair.
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="price">price</param>
		/// <param name="quantity">quantity</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order result with order id</returns>
		public WebCallResult<LiveCoinOrderResult> BuyLimitOrder(string symbol, decimal price, decimal quantity, CancellationToken ct = default) => BuyLimitOrderAsync(symbol, price, quantity, ct).Result;

		/// <summary>
		/// Open a sell order (limit) for particular currency pair.
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="price">price</param>
		/// <param name="quantity">quantity</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order result with order id</returns>
		public async Task<WebCallResult<LiveCoinOrderResult>> SellLimitOrderAsync(string symbol, decimal price, decimal quantity, CancellationToken ct = default)
		{
			symbol.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object> { { "currencyPair", symbol },
				{"price", price },
				{"quantity", quantity } };
			return await SendRequest<LiveCoinOrderResult>(GetUrl(ExchangeSellLimitEndpoint), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Open a sell order (limit) for particular currency pair.
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="price">price</param>
		/// <param name="quantity">quantity</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order result with order id</returns>
		public WebCallResult<LiveCoinOrderResult> SellLimitOrder(string symbol, decimal price, decimal quantity, CancellationToken ct = default) => SellLimitOrderAsync(symbol, price, quantity, ct).Result;


		/// <summary>
		/// Open a buy order (market) for particular currency pair.
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="quantity">quantity</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order result with order id</returns>
		public async Task<WebCallResult<LiveCoinOrderResult>> BuyMarketOrderAsync(string symbol, decimal quantity, CancellationToken ct = default)
		{
			symbol.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object> { { "currencyPair", symbol },
				{"quantity", quantity } };
			return await SendRequest<LiveCoinOrderResult>(GetUrl(ExchangeBuyMarketEndpoint), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Open a buy order (market) for particular currency pair.
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="quantity">quantity</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order result with order id</returns>
		public WebCallResult<LiveCoinOrderResult> BuyMarketOrder(string symbol, decimal quantity, CancellationToken ct = default) => BuyMarketOrderAsync(symbol, quantity, ct).Result;

		/// <summary>
		/// Open a sell order (market) for particular currency pair.
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="quantity">quantity</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order result with order id</returns>
		public async Task<WebCallResult<LiveCoinOrderResult>> SellMarketOrderAsync(string symbol, decimal quantity, CancellationToken ct = default)
		{
			symbol.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object> { { "currencyPair", symbol },
				{"quantity", quantity } };
			return await SendRequest<LiveCoinOrderResult>(GetUrl(ExchangeSellMarketEndpoint), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Open a sell order (market) for particular currency pair.
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="quantity">quantity</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>order result with order id</returns>
		public WebCallResult<LiveCoinOrderResult> SellMarketOrder(string symbol, decimal quantity, CancellationToken ct = default) => SellMarketOrderAsync(symbol, quantity, ct).Result;

		/// <summary>
		/// Cancel order
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="orderId">the order id</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>cancellation request result</returns>
		public async Task<WebCallResult<LiveCoinCancelResult>> CancelLimitOrderAsync(string symbol, long orderId, CancellationToken ct = default)
		{
			symbol.ValidateLiveCoinSymbol();
			var parameters = new Dictionary<string, object> { { "currencyPair", symbol },
				{"orderId", orderId} };
			return await SendRequest<LiveCoinCancelResult>(GetUrl(ExchangeCancelLimitEndpoint), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
		}
		/// <summary>
		/// Cancel order
		/// </summary>
		/// <param name="symbol">currencyPair</param>
		/// <param name="orderId">the order id</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns>cancellation request result</returns>
		public WebCallResult<LiveCoinCancelResult> CancelLimitOrder(string symbol, long orderId, CancellationToken ct = default) => CancelLimitOrderAsync(symbol, orderId, ct).Result;

		#endregion
		#region deposit and withdrawal
		/// <summary>
		/// Get deposit address for selected cryptocurrency.
		/// </summary>
		/// <param name="currency">Currency code</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns> deposit address for selected cryptocurrency
		///		Wallet field has a delimiter "::" for using if you need Memo or Payment ID data besides your wallet to deposit coins(coins like: XMR, BTS, THS, STEEM). In this case wallet data is entered before delimiter, and Memo/Payment ID - after.
		///	</returns>
		public async Task<WebCallResult<LiveCoinAddress>> GetPaymentAddressAsync(string currency, CancellationToken ct = default)
		{
			if (string.IsNullOrWhiteSpace(currency))
				throw new ArgumentNullException(nameof(currency));
			var parameters = new Dictionary<string, object> { { "currency", currency } };
			return await SendRequest<LiveCoinAddress>(GetUrl(PaymentAddressEndPoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
		}
		///<inheritdoc cref="GetPaymentAddressAsync"/>
		public WebCallResult<LiveCoinAddress> GetPaymentAddress(string currency, CancellationToken ct = default) => GetPaymentAddressAsync(currency, ct).Result;

		#endregion
		#region helpers
		private Uri GetUrl(string endpoint)
		{
			var result = $"{BaseAddress}/{endpoint}";
			return new Uri(result);
		}
		/// <inheritdoc />
		protected override Error ParseErrorResponse(JToken error)
		{
			var successToken = error.SelectToken("success");
			if (successToken != null && successToken.Type == JTokenType.Boolean && error.Value<bool>("success") != true)
			{
				var errorCode = error.Value<int>("errorCode");
				var errorMessage = error.Value<string>("errorMessage");
				return new ServerError(errorCode, errorMessage, error);
			}
			return new ServerError(error.ToString());
		}
		/// <inheritdoc />
		protected override Task<ServerError?> TryParseError(JToken data)
		{
			var successToken = data.SelectToken("success");
			if (successToken != null && successToken.Type == JTokenType.Boolean && data.Value<bool>("success") != true)
			{
				var errorCode = data.Value<int>("errorCode");
				var errorMessage = data.Value<string>("errorMessage");
				return Task.FromResult<ServerError?>(new ServerError(errorCode, errorMessage, data));
			}
			return base.TryParseError(data);
		}

		#endregion
		#endregion
	}
}
