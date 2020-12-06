using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Sockets;

namespace LiveCoin.Net.Sockets
{
	/// <summary>
	/// Subscription with server answer
	/// </summary>
	/// <typeparam name="T">Server aswer type</typeparam>
	public class UpdateSubscription<T> : CryptoExchange.Net.Sockets.UpdateSubscription where T : class
	{
		/// <summary>
		/// Server answer
		/// </summary>
		public T? SubscriptionResponse { get; set; }
		internal UpdateSubscription(SocketConnection connection, SocketSubscription subscription, T? response) : base(connection, subscription)
		{
			SubscriptionResponse = response;
		}
	}
}
