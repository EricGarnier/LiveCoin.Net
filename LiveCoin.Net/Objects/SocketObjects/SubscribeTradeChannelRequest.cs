using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class SubscribeTradeChannelRequest
	{
		[global::ProtoBuf.ProtoMember(1)]
		public string? CurrencyPair { get; set; }
	}
}
