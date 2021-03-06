﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class WsResponse
	{
		[global::ProtoBuf.ProtoMember(1)]
		public WsResponseMetaData? Meta { get; set; }

		[global::ProtoBuf.ProtoMember(2)]
		public byte[]? Msg { get; set; }
		public override string ToString()
		{
			return $"{nameof(WsResponse)} Meta:{Meta}, msg length:{Msg?.Length}";
		}

	}
}
