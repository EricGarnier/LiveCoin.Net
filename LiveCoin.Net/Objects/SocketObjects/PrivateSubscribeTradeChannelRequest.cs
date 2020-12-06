using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class PrivateSubscribeTradeChannelRequest : IExpireControl
	{
		[global::ProtoBuf.ProtoMember(1)]
		public RequestExpired? ExpireControl { get; set; }
	}
}
