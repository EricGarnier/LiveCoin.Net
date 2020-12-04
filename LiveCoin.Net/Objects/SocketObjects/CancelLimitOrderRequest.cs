using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Weight is 1 point
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	internal class CancelLimitOrderRequest: IExpireControl
	{
		[global::ProtoBuf.ProtoMember(1)]
		public RequestExpired? ExpireControl { get; set; }

		[global::ProtoBuf.ProtoMember(2)]
		public string? CurrencyPair { get; set; }

		[global::ProtoBuf.ProtoMember(3)]
		public long Id { get; set; }
	}
}
