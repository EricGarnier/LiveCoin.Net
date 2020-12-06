using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Put order limit response
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class PutLimitOrderResponse
	{
		/// <summary>
		/// Order Id
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public long OrderId { get; set; } = 0;
		/// <summary>
		/// quantity not traded yet.
		/// when your order has been fully traded when you put it, this field will be zero
		/// when your order has not been traded at all when you put it, this field will be original order's quantity
		/// when your order has been partially traded when you put it, this field will be the quantity left in orderbook
		/// </summary>
		public decimal AmountLeft { get; set; }
		[global::ProtoBuf.ProtoMember(2)]
		private string AmountLeftImpl { get => AmountLeft.ToString(System.Globalization.CultureInfo.InvariantCulture); set => AmountLeft = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
	}
}
