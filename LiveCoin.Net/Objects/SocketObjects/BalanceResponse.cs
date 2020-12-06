using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Balance event
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class BalanceResponse
	{
		/// <summary>
		/// Balance type
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public BalanceType BalanceType { get; set; } = BalanceType.Total;

		/// <summary>
		/// Currency
		/// </summary>
		[global::ProtoBuf.ProtoMember(2)]
		public string? Currency { get; set; }
		/// <summary>
		/// Value
		/// </summary>
		public decimal Value { get; set; }
		[global::ProtoBuf.ProtoMember(3)]
		private string ValueImpl { get => Value.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Value = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
	}
}
