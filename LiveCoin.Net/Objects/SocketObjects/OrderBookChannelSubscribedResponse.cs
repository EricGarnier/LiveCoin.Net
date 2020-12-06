using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Server answer to subscribe request
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class OrderBookChannelSubscribedResponse
	{
		/// <summary>
		/// Order book channel subscribed
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public string? CurrencyPair { get; set; }
		/// <summary>
		/// current snapshot of orderbook. It is guaranted that all notifications will be from this snapshot.
		/// </summary>

		[global::ProtoBuf.ProtoMember(2)]
		public global::System.Collections.Generic.List<OrderBookEvent> Data { get; } = new global::System.Collections.Generic.List<OrderBookEvent>();
	}
}
