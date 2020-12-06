using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class ChannelUnsubscribedResponse
	{
		[global::ProtoBuf.ProtoMember(1)]
		public ChannelType Type { get; set; } = ChannelType.Ticker;

		[global::ProtoBuf.ProtoMember(2)]
		public string? CurrencyPair { get; set; }
	}
}
