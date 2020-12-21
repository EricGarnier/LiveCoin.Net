using CryptoExchange.Net.Objects;

namespace LiveCoin.Net
{
	/// <summary>
	/// LiveCoin symbol order book options
	/// </summary>
	public class LiveCoinOrderBookOptions: OrderBookOptions
    {
        /// <summary>
        /// The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks. Leaving this null will sync the full order book
        /// </summary>
        public int? Limit { get; }


        /// <summary>
        /// The rest client to use. A new one will be created with default option if not provided.
        /// </summary>
        public LiveCoinClient? RestClient { get; }
        /// <summary>
        /// The socket client to use. A new one will be created with default option if not provided.
        /// </summary>
        public LiveCoinSocketClient? SocketClient { get; }

        /// <summary>
        /// Create new options
        /// </summary>
        /// <param name="limit">The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks. Leaving this null will sync the full order book</param>
        /// <param name="restClient">The rest client to use. A new one will be created with default option if not provided.</param>
        /// <param name="socketClient">The socket client to use. A new one will be created with default option if not provided.</param>
        public LiveCoinOrderBookOptions(int? limit = null, LiveCoinClient? restClient=null, LiveCoinSocketClient? socketClient=null) : base("LiveCoinOrderBook", false, limit != null)
        {
            Limit = limit;
            RestClient = restClient;
            SocketClient = socketClient;
        }
    }
}