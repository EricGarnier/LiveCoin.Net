using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Order
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class Order
	{
        /// <summary>
        /// Id
        /// </summary>
        [global::ProtoBuf.ProtoMember(1)]
        public long Id { get; set; }
        /// <summary>
        /// Currency pair
        /// </summary>
        [global::ProtoBuf.ProtoMember(2)]
        public string? CurrencyPair { get; set; }

        /// <summary>
        /// Good until time
        /// </summary>
        public DateTime GoodUntilTime { get; set; }
        [global::ProtoBuf.ProtoMember(3)]
        private long GoodUntilTimeImpl { get => GoodUntilTime.ToUnixMilliseconds(); set => GoodUntilTime = value.ToUnixMilliseconds(); }
        /// <summary>
        /// Order type
        /// </summary>
        [global::ProtoBuf.ProtoMember(4)]
        public OrderMarketType OrderType { get; set; } = OrderMarketType.MarketBuy;
        /// <summary>
        /// Order status
        /// </summary>
        [global::ProtoBuf.ProtoMember(5)]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.New;
        /// <summary>
        /// Order issue time
        /// </summary>
        public DateTime IssueTime { get; set; }
        [global::ProtoBuf.ProtoMember(6)]
        private long IssueTimeImpl { get => IssueTime.ToUnixMilliseconds(); set => IssueTime = value.ToUnixMilliseconds(); }
        /// <summary>
        /// Price
        /// </summary>
		public decimal? Price { get; set; }
        [global::ProtoBuf.ProtoMember(7)]
        private string? PriceImpl
        {
            get => Price?.ToString(System.Globalization.CultureInfo.InvariantCulture);
            set => Price = (value != null) ? (decimal?)(decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture)) : (decimal?)null;
        }
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        [global::ProtoBuf.ProtoMember(8)]
        private string QuantityImpl { get => Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Quantity = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Remaining quantity
        /// </summary>
        public decimal RemainingQuantity { get; set; }
        [global::ProtoBuf.ProtoMember(9)]
        private string RemainingQuantityImpl { get => RemainingQuantity.ToString(System.Globalization.CultureInfo.InvariantCulture); set => RemainingQuantity  = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Commission by trade
        /// </summary>
        public decimal CommissionByTrade { get; set; }
        [global::ProtoBuf.ProtoMember(10)]
        private string CommissionByTradeImpl { get => CommissionByTrade.ToString(System.Globalization.CultureInfo.InvariantCulture); set => CommissionByTrade = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Bonus by trade
        /// </summary>
        public decimal BonusByTrade { get; set; }
        [global::ProtoBuf.ProtoMember(11)]
        private string BonusByTradeImpl { get => BonusByTrade.ToString(System.Globalization.CultureInfo.InvariantCulture); set => BonusByTrade = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Bonus rate
        /// </summary>
        public decimal BonusRate { get; set; }
        [global::ProtoBuf.ProtoMember(12)]
        private string BonusRateImpl { get => BonusRate.ToString(System.Globalization.CultureInfo.InvariantCulture); set => BonusRate = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Commission rate
        /// </summary>
        public decimal CommissionRate { get; set; }
        [global::ProtoBuf.ProtoMember(13)]
        private string CommissionRateImpl { get => CommissionRate.ToString(System.Globalization.CultureInfo.InvariantCulture); set => CommissionRate = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Last modification time
        /// </summary>
        public DateTime LastModificationTime { get; set; }
        [global::ProtoBuf.ProtoMember(14)]
        private long LastModificationTimeImpl { get => LastModificationTime.ToUnixMilliseconds(); set => LastModificationTime = value.ToUnixMilliseconds(); }
    }
}
