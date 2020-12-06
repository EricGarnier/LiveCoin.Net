using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class PrivateChannelUnsubscribedResponse
	{
		[global::ProtoBuf.ProtoMember(1)]
		public PrivateChannelType PrivateChannelType { get; set; } = PrivateChannelType.OrderRaw;
	}
}
