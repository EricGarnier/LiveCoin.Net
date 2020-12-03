using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Test
{
	[global::ProtoBuf.ProtoContract()]
	class ChannelUnsubscribedResponse
	{
		[global::ProtoBuf.ProtoMember(1)]
		public UnsubscribeRequest.ChannelType Type { get; set; } = UnsubscribeRequest.ChannelType.Ticker;

		[global::ProtoBuf.ProtoMember(2)]
		public string? CurrencyPair { get; set; }
	}
}
