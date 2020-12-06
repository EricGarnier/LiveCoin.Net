using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Private raw order event
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class PrivateOrderRawEvent
	{
        /// <summary>
        /// Order type
        /// </summary>
        [global::ProtoBuf.ProtoMember(1)]
        public OrderType OrderType { get; set; } = OrderType.Bid;
        /// <summary>
        /// Order id
        /// </summary>
        [global::ProtoBuf.ProtoMember(2)]
        public long Id { get; set; }
        /// <summary>
        /// timestamp of the last order's change
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
        /// zero when order is removed from orderbook (cancelled or traded)
        /// </summary>
        public decimal Quantity { get; set; }
        [global::ProtoBuf.ProtoMember(5)]
        private string QuantityImpl { get => Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Quantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Currency pair
        /// </summary>
        [global::ProtoBuf.ProtoMember(6)]
        public string? CurrencyPair { get; set; }
        /// <summary>
        /// Is market
        /// </summary>
        [global::ProtoBuf.ProtoMember(7)]
        public bool IsMarket { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal QuantityLeftBeforeCancellation { get; set; }
        [global::ProtoBuf.ProtoMember(8)]
        private string QuantityLeftBeforeCancellationImpl { get => QuantityLeftBeforeCancellation.ToString(System.Globalization.CultureInfo.InvariantCulture); set => QuantityLeftBeforeCancellation = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
    }
}
