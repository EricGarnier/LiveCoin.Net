using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{

	/// <summary>
	/// Cancel orders response
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class CancelOrdersResponse
	{
		/// <summary>
		/// Cancelled orders
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public global::System.Collections.Generic.List<OrderCancelled> Orders { get; } = new global::System.Collections.Generic.List<OrderCancelled>();
	}
}
