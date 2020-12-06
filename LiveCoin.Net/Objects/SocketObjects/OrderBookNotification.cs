using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// ORDER_BOOK_NOTIFY
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class OrderBookNotification
	{
		/// <summary>
		/// Currency pair
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public string? CurrencyPair { get; set; }
		/// <summary>
		/// Notification details
		/// </summary>

		[global::ProtoBuf.ProtoMember(2)]
		public global::System.Collections.Generic.List<OrderBookEvent> Data { get; } = new global::System.Collections.Generic.List<OrderBookEvent>();

	}
}
