using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Interfaces;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Orderbook event
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class OrderBookEvent : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Order type
        /// </summary>
        [global::ProtoBuf.ProtoMember(1)]
        public OrderBidAskType OrderType { get; set; } = OrderBidAskType.Bid;
        /// <summary>
        /// Order timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
        [global::ProtoBuf.ProtoMember(2)]
        long TimestampImpl { get => Timestamp.ToUnixMilliseconds(); set => Timestamp = value.ToUnixMilliseconds(); }
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
        [global::ProtoBuf.ProtoMember(3)]
        private string PriceImpl { get => Price.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Price = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        [global::ProtoBuf.ProtoMember(4)]
        private string QuantityImpl { get => Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Quantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }

    }
}
