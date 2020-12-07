using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class CancelOrdersRequest: IExpireControl
	{
		[global::ProtoBuf.ProtoMember(1)]
		public RequestExpired? ExpireControl { get; set; }

		[global::ProtoBuf.ProtoMember(2)]
		public global::System.Collections.Generic.List<string> CurrencyPairs { get; } = new global::System.Collections.Generic.List<string>();
	}
}
