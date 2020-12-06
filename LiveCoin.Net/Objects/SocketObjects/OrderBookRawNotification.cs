using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Order book raw notification
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class OrderBookRawNotification
	{
		/// <summary>
		/// Currency pait
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public string? CurrencyPair { get; set; }

		/// <summary>
		/// Event data
		/// </summary>
		[global::ProtoBuf.ProtoMember(2)]
		public global::System.Collections.Generic.List<OrderBookRawEvent> Data { get; } = new global::System.Collections.Generic.List<OrderBookRawEvent>();

	}
}
