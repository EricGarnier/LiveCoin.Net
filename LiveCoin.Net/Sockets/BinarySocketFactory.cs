using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;

namespace LiveCoin.Net.Sockets
{
	internal class BinarySocketFactory : IWebsocketFactory
    {
        /// <inheritdoc />
        public IWebsocket CreateWebsocket(Log log, string url)
        {
            return new BinarySocket(log, url);
        }

        /// <inheritdoc />
        public IWebsocket CreateWebsocket(Log log, string url, IDictionary<string, string> cookies, IDictionary<string, string> headers)
        {
            return new BinarySocket(log, url, cookies, headers);
        }
    }
}
