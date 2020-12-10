using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	[global::ProtoBuf.ProtoContract()]
	internal class ClientOrdersRequest : IExpireControl
	{
		[global::ProtoBuf.ProtoMember(1)]
		public RequestExpired? ExpireControl { get; set; }
		[global::ProtoBuf.ProtoMember(2)]
		public string? CurrencyPair { get; set; }
		[global::ProtoBuf.ProtoMember(3)]
		public FilterOrderStatus? Status { get; set; }
		public DateTime? IssuedFrom { get; set; }
		[global::ProtoBuf.ProtoMember(4)]
		private long? IssuedFromImpl { get => IssuedFrom?.ToUnixMilliseconds(); set => IssuedFrom = value?.ToUnixMilliseconds(); }
		public DateTime? IssuedTo { get; set; }
		[global::ProtoBuf.ProtoMember(5)]
		private long? IssuedToImpl { get => IssuedTo?.ToUnixMilliseconds(); set => IssuedTo = value?.ToUnixMilliseconds(); }
		[global::ProtoBuf.ProtoMember(6)]
		public OrderBidAskType? OrderType { get; set; }
		[global::ProtoBuf.ProtoMember(7)]
		public int? StartRow { get; set; }
		[global::ProtoBuf.ProtoMember(8)]
		public int? EndRow { get; set; }
	}
}
