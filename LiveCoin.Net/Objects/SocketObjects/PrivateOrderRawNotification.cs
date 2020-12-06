using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Private raw order notification or subscription
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class PrivateOrderRawNotification
	{
		/// <summary>
		/// The private raw order events
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public global::System.Collections.Generic.List<PrivateOrderRawEvent> Data { get; } = new global::System.Collections.Generic.List<PrivateOrderRawEvent>();
	}
}
