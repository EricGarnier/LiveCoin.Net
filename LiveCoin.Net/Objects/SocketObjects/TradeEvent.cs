﻿using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Enums;

namespace LiveCoin.Net.Objects.SocketObjects
{
    /// <summary>
    /// A single trade event
    /// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class TradeEvent
	{
        /// <summary>
        /// Trade Id
        /// </summary>
        [global::ProtoBuf.ProtoMember(1)]
        public long Id { get; set; }
        /// <summary>
        /// Trade type
        /// </summary>
        [global::ProtoBuf.ProtoMember(2)]
        public Side Side{ get; set; } = Side.Buy;
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
        [global::ProtoBuf.ProtoMember(3)]
        private long TimestampImpl { get => Timestamp.ToUnixMilliseconds(); set => Timestamp = value.ToUnixMilliseconds(); }
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
        [global::ProtoBuf.ProtoMember(4)]
        private string PriceImpl { get => Price.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Price = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        [global::ProtoBuf.ProtoMember(5)]
        private string QuantityImpl { get => Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Quantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Order buy id
        /// </summary>
        [global::ProtoBuf.ProtoMember(6)]
        public long? OrderBuyId { get; set; }
        /// <summary>
        /// Order sell id
        /// </summary>
        [global::ProtoBuf.ProtoMember(7)]
        public long? OrderSellId { get; set; }
    }
}
