using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Sockets;
using SuperSocket.ClientEngine.Proxy;
using WebSocket4Net;

namespace LiveCoin.Net.Sockets
{
	internal class BinarySocket : BaseSocket
	{
        private readonly IDictionary<string, string> cookies;
        private readonly IDictionary<string, string> headers;
        private HttpConnectProxy? proxy;
        /// <inheritdoc />
        public BinarySocket(Log log, string url) : this(log, url, new Dictionary<string, string>(), new Dictionary<string, string>())
        {
        }

        /// <inheritdoc />
        public BinarySocket(Log log, string url, IDictionary<string, string> cookies, IDictionary<string, string> headers) : base(log, url, cookies, headers)
        {
            this.cookies = cookies;
            this.headers = headers;
        }

        /// <inheritdoc />
		public override void Send(string data)
		{
            const string startString = @"""Data"":""";
            var startIndex = data.IndexOf(startString);
            if (startIndex <= 0)
			{
                throw new ArgumentOutOfRangeException(nameof(data), $"Can't find '{startString}' in '{data}'");
            }
            var endIndex = data.IndexOf('"', startIndex + startString.Length);
            if (endIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"Can't find end quote after {startIndex} in '{data}'");
            }
            var binary = System.Convert.FromBase64String(data.Substring(startIndex + startString.Length, endIndex - (startIndex + startString.Length)));
            socket?.Send(binary, 0, binary.Length);
		}

		/// <inheritdoc />
		public override Task<bool> Connect()
		{
            if (socket == null)
            {
                socket = new WebSocket(Url, cookies: cookies.ToList(), customHeaderItems: headers.ToList(), origin: Origin ?? "")
                {
                    EnableAutoSendPing = true,
                    AutoSendPingInterval = 10
                };

                if (proxy != null)
                    socket.Proxy = proxy;

                socket.Security.EnabledSslProtocols = SSLProtocols;
                socket.Opened += (o, s) => Handle(openHandlers);
                socket.Closed += (o, s) => Handle(closeHandlers);
                socket.Error += (o, s) => Handle(errorHandlers, s.Exception);
                socket.MessageReceived += (o, s) => HandleStringData(s.Message);
                socket.DataReceived += (o, s) => HandleByteData(s.Data);
            }
            return base.Connect();
		}

        /// <inheritdoc />
        public override void SetProxy(string host, int port)
        {
            proxy = IPAddress.TryParse(host, out var address)
                ? new HttpConnectProxy(new IPEndPoint(address, port))
                : new HttpConnectProxy(new DnsEndPoint(host, port));
            base.SetProxy(host, port);
        }
        private void HandleByteData(byte[] data)
        {
            try
            {
                var sb = new StringBuilder(data.Length * 2 + 20);
                sb.Append(@"{""Data"":""");
                sb.Append(System.Convert.ToBase64String(data));
                sb.Append(@"""}");
                var message = sb.ToString();
                Handle(messageHandlers, message);
            }
            catch (Exception ex)
            {
                log.Write(LogVerbosity.Error, $"{Id} Something went wrong while processing a byte message from the socket: {ex}");
            }
        }

        private void HandleStringData(string data)
        {
            try
            {
                Handle(messageHandlers, data);
            }
            catch (Exception ex)
            {
                log.Write(LogVerbosity.Error, $"{Id} Something went wrong while processing a string message from the socket: {ex}");
            }
        }

    }
}
