﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Trade notification
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class TradeNotification
	{
		/// <summary>
		/// Currency pair
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public string? CurrencyPair { get; set; }
		/// <summary>
		/// Trade details
		/// </summary>
		[global::ProtoBuf.ProtoMember(2)]
		public global::System.Collections.Generic.List<TradeEvent> Data { get; } = new global::System.Collections.Generic.List<TradeEvent>();
	}
}
