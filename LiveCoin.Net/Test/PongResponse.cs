using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Test
{
	[global::ProtoBuf.ProtoContract()]
	public class PongResponse
	{
		public DateTime PingTime { get; set; }
		[global::ProtoBuf.ProtoMember(1)]
		long PingTimeImpl { get => PingTime.ToUnixMilliseconds(); set => PingTime = value.ToUnixMilliseconds(); }
	}
}
