using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    [global::ProtoBuf.ProtoContract()]
    internal class SubscribeTickerChannelRequest
    {
        [global::ProtoBuf.ProtoMember(1)]
        public string? CurrencyPair { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public float? Frequency { get; set; }
    }
}
