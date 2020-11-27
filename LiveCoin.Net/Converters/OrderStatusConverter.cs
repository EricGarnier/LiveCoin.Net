using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Enums;

namespace LiveCoin.Net.Converters
{
	internal class OrderStatusConverter : BaseConverter<OrderStatus>
    {
        public OrderStatusConverter() : this(true) { }
        public OrderStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderStatus, string>> Mapping => new List<KeyValuePair<OrderStatus, string>>
        {
            new KeyValuePair<OrderStatus, string>(OrderStatus.New, "NEW"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Open, "OPEN"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Expired, "EXPIRED"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Cancelled, "CANCELLED"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Executed, "EXECUTED"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.PartiallyFilled, "PARTIALLY_FILLED"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.PartiallyFilledAndCancelled, "PARTIALLY_FILLED_AND_CANCELLED"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.PartiallyFilledAndExpired, "PARTIALLY_FILLED_AND_EXPIRED"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.UnknownStatus, "UNKNOWN_STATUS")
        };
    }
}
