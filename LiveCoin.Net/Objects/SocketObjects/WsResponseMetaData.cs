using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    [global::ProtoBuf.ProtoContract()]
	internal class WsResponseMetaData
	{
        [global::ProtoBuf.ProtoMember(1)]
        public WsResponseMsgType ResponseType { get; set; } = WsResponseMsgType.TickerChannelSubscribed;

        [global::ProtoBuf.ProtoMember(2)]
        public string Token { get; set; } = string.Empty;
    }
}
