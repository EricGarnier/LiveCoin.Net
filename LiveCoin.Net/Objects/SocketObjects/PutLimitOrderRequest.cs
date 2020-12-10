using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Put limit order request - private api, you have to be logged in
	/// Weight is 1 point
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	internal class PutLimitOrderRequest : IExpireControl
	{
		[global::ProtoBuf.ProtoMember(1)]
		public RequestExpired? ExpireControl { get; set; }

		[global::ProtoBuf.ProtoMember(2)]
		public string? CurrencyPair { get; set; }

		[global::ProtoBuf.ProtoMember(3)]
		public OrderBidAskType OrderType { get; set; } = OrderBidAskType.Bid;
		public decimal Amount { get; set; }
		[global::ProtoBuf.ProtoMember(4)]
		private string AmountImpl { get => Amount.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Amount = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }

		public decimal Price { get; set; }
		[global::ProtoBuf.ProtoMember(5)]
		private string PriceImpl { get => Price.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Price = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
	}
}
