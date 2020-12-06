using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Cancle notification
	/// </summary>
	[global::ProtoBuf.ProtoContract()]

	public class CandleNotification
	{
		/// <summary>
		/// Currency pair
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public string? CurrencyPair { get; set; }

		/// <summary>
		/// The candle intgerval
		/// </summary>
		[global::ProtoBuf.ProtoMember(2)]
		public CandleInterval Interval { get; set; } = CandleInterval.Candle1Minute;
		/// <summary>
		/// The candle events
		/// </summary>
		[global::ProtoBuf.ProtoMember(3)]
		public global::System.Collections.Generic.List<CandleEvent> Data { get; } = new global::System.Collections.Generic.List<CandleEvent>();
	}
}
