using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class SubscribeCandleChannelRequest
	{
        [global::ProtoBuf.ProtoMember(1)]
        public string? CurrencyPair { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public CandleInterval Interval { get; set; } = CandleInterval.Candle1Minute;

        [global::ProtoBuf.ProtoMember(3)]
        public int? Depth { get; set; }

    }
}
