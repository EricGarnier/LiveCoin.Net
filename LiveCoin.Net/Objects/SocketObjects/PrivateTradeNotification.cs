using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Private trade notification or subscription
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class PrivateTradeNotification
	{
		/// <summary>
		/// Event details
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public global::System.Collections.Generic.List<PrivateTradeEvent> Data { get; } = new global::System.Collections.Generic.List<PrivateTradeEvent>();
	}
}
