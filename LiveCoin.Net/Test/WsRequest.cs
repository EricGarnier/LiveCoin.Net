using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Test
{
	[global::ProtoBuf.ProtoContract()]
	class WsRequest
	{
		[global::ProtoBuf.ProtoMember(1)]
		public WsRequestMetaData Meta { get; set; }

		[global::ProtoBuf.ProtoMember(2)]
		public byte[] Msg { get; set; }
	}
}
