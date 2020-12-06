using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    [global::ProtoBuf.ProtoContract()]
	internal class WsRequestMetaData
	{
        [global::ProtoBuf.ProtoMember(1)]
        public WsRequestMsgType RequestType { get; set; } = WsRequestMsgType.SubscribeTicker;

        [global::ProtoBuf.ProtoMember(2)]
        public string Token { get; set; } = string.Empty;

        [global::ProtoBuf.ProtoMember(4)]
        public byte[]? Sign { get; set; }

    }
}
