using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class ClientOrderRequest: IExpireControl
    {
        [global::ProtoBuf.ProtoMember(1)]
        public RequestExpired? ExpireControl { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public long OrderId { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public string? CurrencyPair { get; set; }

    }
}
