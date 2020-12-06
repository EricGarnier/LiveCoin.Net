using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Ticker notification
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class TickerNotification
	{
		/// <summary>
		/// Currency pair
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public string? CurrencyPair { get; set; }
		/// <summary>
		/// Ticker events
		/// </summary>
		[global::ProtoBuf.ProtoMember(2)]
		public global::System.Collections.Generic.List<TickerEvent> Data { get; } = new global::System.Collections.Generic.List<TickerEvent>();
	}
}
