using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Test
{
	[global::ProtoBuf.ProtoContract()]
	public class TickerNotification
	{
		[global::ProtoBuf.ProtoMember(1)]
		public string CurrencyPair { get; set; }

		[global::ProtoBuf.ProtoMember(2)]
		public global::System.Collections.Generic.List<TickerEvent> Datas { get; } = new global::System.Collections.Generic.List<TickerEvent>();

	}
}
