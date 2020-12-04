using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class RequestExpired
	{
		/// <summary>
		/// current Epoch timestamp
		/// </summary>
		public DateTime Now { get; set; }
		[global::ProtoBuf.ProtoMember(1)]
		public long NowImpl { get => Now.ToUnixMilliseconds(); set => Now = value.ToUnixMilliseconds(); }
		/// <summary>
		/// 'Time to live' milliseconds
		/// </summary>
		[global::ProtoBuf.ProtoMember(2)]
		public int Ttl { get; set; }
	}
}
