using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Order book raw event
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class OrderBookRawEvent
	{
		/// <summary>
		/// Order type
		/// </summary>
		[global::ProtoBuf.ProtoMember(1)]
		public OrderType OrderType { get; set; } = OrderType.Bid;

		/// <summary>
		/// Order entry id
		/// </summary>
		[global::ProtoBuf.ProtoMember(2)]
		public long Id { get; set; }

		/// <summary>
		/// Order timestamp
		/// </summary>
		public DateTime Timestamp { get; set; }
		[global::ProtoBuf.ProtoMember(3)]
		private long TimestampImpl { get => Timestamp.ToUnixMilliseconds(); set => Timestamp = value.ToUnixMilliseconds(); }
		/// <summary>
		/// Price
		/// </summary>
		public decimal? Price { get; set; }
		[global::ProtoBuf.ProtoMember(4)]
		private string? PriceImpl
		{
			get => Price?.ToString(System.Globalization.CultureInfo.InvariantCulture);
			set => Price = (value != null) ? (decimal?)(decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture)) : (decimal?)null;
		}
		/// <summary>
		/// Quantity
		/// </summary>
		public decimal Quantity { get; set; }
		[global::ProtoBuf.ProtoMember(5)]
		private string QuantityImpl { get => Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Quantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
	}
}
