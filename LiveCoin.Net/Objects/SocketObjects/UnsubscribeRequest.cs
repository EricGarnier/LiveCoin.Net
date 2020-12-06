using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    [global::ProtoBuf.ProtoContract()]
	internal class UnsubscribeRequest
	{
        [global::ProtoBuf.ProtoMember(1)]
        public ChannelType ChannelType { get; set; } = ChannelType.Ticker;

        [global::ProtoBuf.ProtoMember(2)]
        public string? CurrencyPair { get; set; }

    }
}
