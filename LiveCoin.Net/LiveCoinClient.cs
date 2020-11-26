using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using LiveCoin.Net.Objects;

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
		private const string OrderbookEndpoint = "exchange/order_book";
		private const string TradesHistoryEndpoint = "exchange/last_trades";
		private const string ExchangeRestrictionsEndpoint = "exchange/restrictions";


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

		#endregion
		#endregion
		#region helpers
		private Uri GetUrl(string endpoint)
		{
			var result = $"{BaseAddress}/{endpoint}";
			return new Uri(result);
		}
		#endregion
	}
}
