using System;
using System.Collections.Generic;
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
		private const string PaymentBalancesEndpoint = "payment/balances";
		private const string ExchangeBuyLimitEndpoint = "exchange/buylimit";
		private const string ExchangeSellLimitEndpoint = "exchange/selllimit";
		private const string ExchangeCancelLimitEndpoint = "exchange/cancellimit";
		private const string ExchangeClientOrdersEndpoint = "exchange/client_orders";
		private const string ExchangeCommissionEndpoint = "exchange/commission";
		private const string ExchangeOrderEndpoint = "exchange/order";


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
		public LiveCoinClient(LiveCoinClientOptions options) : base(options, options.ApiCredentials == null ? null : new LiveCoinAuthenticationProvider(options.ApiCredentials, ArrayParametersSerialization.MultipleValues))
		{
			manualParseError = true;
			arraySerialization = ArrayParametersSerialization.MultipleValues;
			requestBodyFormat = RequestBodyFormat.FormData;
		}
		#endregion

		#region methods
		#region public

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
			SetAuthenticationProvider(new LiveCoinAuthenticationProvider(new ApiCredentials(apiKey, apiSecret), ArrayParametersSerialization.MultipleValues));
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
	}
}
