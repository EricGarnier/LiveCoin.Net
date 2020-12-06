using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Ticker channel subscribe response
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class TickerChannelSubscribedResponse
	{
		/// <summary>
		/// The currency pair
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public string? CurrencyPair { get; set; }
		/// <summary>
		/// Events
		/// </summary>
		[global::ProtoBuf.ProtoMember(2)]
		public List<TickerEvent> Data { get; } = new List<TickerEvent>();

	}
}
