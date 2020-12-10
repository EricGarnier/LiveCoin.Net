using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Order detail
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class ClientOrderResponse
	{
        /// <summary>
        /// Order id
        /// </summary>
        [global::ProtoBuf.ProtoMember(1)]
        public long Id { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [global::ProtoBuf.ProtoMember(2)]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.New;
        /// <summary>
        /// Currency pair
        /// </summary>
        [global::ProtoBuf.ProtoMember(3)]
        public string? currencyPair { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price{ get; set; }
        [global::ProtoBuf.ProtoMember(4)]
        private string PriceImpl { get => Price.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Price = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        [global::ProtoBuf.ProtoMember(5)]
        private string QuantityImpl { get => Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Quantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Remaining quantity
        /// </summary>
        public decimal RemainingQuantity { get; set; }
        [global::ProtoBuf.ProtoMember(6)]
        private string RemainingQuantityImpl { get => RemainingQuantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => RemainingQuantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Blocked
        /// </summary>
        public decimal Blocked { get; set; }
        [global::ProtoBuf.ProtoMember(7)]
        private string BlockedImpl { get => Blocked.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Blocked = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Blocked remain
        /// </summary>
        public decimal BlockedRemain { get; set; }
        [global::ProtoBuf.ProtoMember(8)]
        private string BlockedRemainImpl { get => BlockedRemain.ToString(System.Globalization.CultureInfo.InvariantCulture); set => BlockedRemain = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Trades
        /// </summary>
        [global::ProtoBuf.ProtoMember(10)]
        public Trades? Trades { get; set; }

    }
}
