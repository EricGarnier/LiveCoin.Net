using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Enums;

namespace LiveCoin.Net.Converters
{
	internal class OrderStatusFilterConverter : BaseConverter<OrderStatusFilter>
    {
        public OrderStatusFilterConverter() : this(true) { }
        public OrderStatusFilterConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderStatusFilter, string>> Mapping => new List<KeyValuePair<OrderStatusFilter, string>>
        {
            new KeyValuePair<OrderStatusFilter, string>(OrderStatusFilter.All, "ALL"),
            new KeyValuePair<OrderStatusFilter, string>(OrderStatusFilter.Open, "OPEN"),
            new KeyValuePair<OrderStatusFilter, string>(OrderStatusFilter.Closed, "CLOSED"),
            new KeyValuePair<OrderStatusFilter, string>(OrderStatusFilter.Cancelled, "CANCELLED"),
            new KeyValuePair<OrderStatusFilter, string>(OrderStatusFilter.NotCancelled, "NOT_CANCELLED"),
            new KeyValuePair<OrderStatusFilter, string>(OrderStatusFilter.Partially, "PARTIALLY")
        };
    }
}
