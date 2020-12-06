using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Subscription response from the server
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class OrderBookRawChannelSubscribedResponse
	{
		/// <summary>
		/// Currency pair
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public string? CurrencyPair { get; set; }

		/// <summary>
		/// current snapshot of orderbook. It is guaranted that all notifications will be from this snapshot.
		/// </summary>
		[global::ProtoBuf.ProtoMember(2)]
		public global::System.Collections.Generic.List<OrderBookRawEvent> Data { get; } = new global::System.Collections.Generic.List<OrderBookRawEvent>();

	}
}
