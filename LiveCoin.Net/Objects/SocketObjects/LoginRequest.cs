﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class LoginRequest : IExpireControl
	{
		[global::ProtoBuf.ProtoMember(1)]
		public RequestExpired? ExpireControl { get; set; }

		[global::ProtoBuf.ProtoMember(2)]
		public string? ApiKey { get; set; }
	}
}
