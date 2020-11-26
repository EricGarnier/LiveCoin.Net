using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Objects;

namespace LiveCoin.Net.Objects
{
    /// <summary>
    /// Options for the LiveCoin client
    /// </summary>
    public class LiveCoinClientOptions : RestClientOptions
    {
        /// <summary>
        /// ctor
        /// </summary>
        public LiveCoinClientOptions() : base("https://api.livecoin.net")
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseAddress">Сustom url to connect via mirror website</param>
        public LiveCoinClientOptions(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Return a copy of these options
        /// </summary>
        /// <returns></returns>
        public LiveCoinClientOptions Copy()
        {
            var copy = Copy<LiveCoinClientOptions>();
            return copy;
        }
    }
}
