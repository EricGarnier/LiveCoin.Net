using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Objects;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// LiveCoin socket client options
	/// </summary>
	public class LiveCoinSocketClientOptions : SocketClientOptions
	{
        /// <summary>
        /// ctor
        /// </summary>
        public LiveCoinSocketClientOptions() : base("wss://ws.api.livecoin.net/ws/beta2")
        {
        }

        /// <summary>
        /// Return a copy of these options
        /// </summary>
        /// <returns></returns>
        public LiveCoinSocketClientOptions Copy()
        {
            var copy = Copy<LiveCoinSocketClientOptions>();
            return copy;
        }
    }
}
