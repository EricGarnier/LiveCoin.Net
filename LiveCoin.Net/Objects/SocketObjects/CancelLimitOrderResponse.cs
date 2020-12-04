using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects

{
	/// <summary>
	/// CANCEL_LIMIT_ORDER_RESPONSE
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class CancelLimitOrderResponse
	{
		/// <summary>
		/// Order id
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public long OrderId { get; set; }

		/// <summary>
		/// quantity not traded yet. Never can be zero :)
		/// </summary>
		public decimal AmountLeft { get; set; }
		[global::ProtoBuf.ProtoMember(2)]
		private string AmountLeftImpl { get => AmountLeft.ToString(System.Globalization.CultureInfo.InvariantCulture); set => AmountLeft = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }

	}
}
