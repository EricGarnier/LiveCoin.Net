using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Test
{
	[global::ProtoBuf.ProtoContract()]
	class WsResponse
	{
		[global::ProtoBuf.ProtoMember(1)]
		public WsResponseMetaData Meta { get; set; }

		[global::ProtoBuf.ProtoMember(2)]
		public byte[] Msg { get; set; }

	}
}
