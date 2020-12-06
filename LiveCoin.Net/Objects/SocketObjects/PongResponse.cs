using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Pong response
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class PongResponse
	{
		/// <summary>
		/// time of received ping
		/// </summary>
		public DateTime PingTime { get; set; }
		[global::ProtoBuf.ProtoMember(1)]
		private long PingTimeImpl { get => PingTime.ToUnixMilliseconds(); set => PingTime = value.ToUnixMilliseconds(); }
	}
}
