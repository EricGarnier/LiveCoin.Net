using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Changebalance notification or subscription
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class PrivateChangeBalanceNotification
	{
		/// <summary>
		/// Details
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public global::System.Collections.Generic.List<BalanceResponse> Data { get; } = new global::System.Collections.Generic.List<BalanceResponse>();
	}
}
