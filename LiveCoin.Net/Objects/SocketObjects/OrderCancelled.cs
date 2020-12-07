using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Order cancelled
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class OrderCancelled
	{
		/// <summary>
		/// Order id
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public long Id { get; set; }
		/// <summary>
		/// Quantity
		/// </summary>
		public decimal Quantity { get; set; }
		[global::ProtoBuf.ProtoMember(2)]
		private string QuantityImpl { get => Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Quantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Trade quantity
		/// </summary>
		public decimal TradeQuantity { get; set; }
		[global::ProtoBuf.ProtoMember(3)]
		private string TradeQuantityImpl { get => TradeQuantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => TradeQuantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
	}
}
