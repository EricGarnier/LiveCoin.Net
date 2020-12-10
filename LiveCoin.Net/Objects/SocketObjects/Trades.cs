using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Trades 
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class Trades
	{
		/// <summary>
		/// Trades
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public int NbTrades { get; set; }
		/// <summary>
		/// Amount
		/// </summary>
		public decimal Amount { get; set; }
		[global::ProtoBuf.ProtoMember(2)]
		private string AmountImpl { get => Amount.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Amount = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Quantity
		/// </summary>
		public decimal Quantity { get; set; }
		[global::ProtoBuf.ProtoMember(3)]
		private string QuantityImpl { get => Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Quantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Average price
		/// </summary>
		public decimal AvgPrice { get; set; }
		[global::ProtoBuf.ProtoMember(4)]
		private string AvgPriceImpl { get => AvgPrice.ToString(System.Globalization.CultureInfo.InvariantCulture); set => AvgPrice = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Commission
		/// </summary>
		public decimal Commission{ get; set; }
		[global::ProtoBuf.ProtoMember(5)]
		private string CommissionImpl { get => Commission.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Commission = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Bonus
		/// </summary>
		public decimal Bonus{ get; set; }
		[global::ProtoBuf.ProtoMember(6)]
		private string BonusImpl { get => Bonus.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Bonus= decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
	}
}
