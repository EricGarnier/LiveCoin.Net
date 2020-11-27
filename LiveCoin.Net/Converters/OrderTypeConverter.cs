using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Enums;

namespace LiveCoin.Net.Converters
{
	internal class OrderTypeConverter : BaseConverter<OrderType>
    {
        public OrderTypeConverter() : this(true) { }
        public OrderTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderType, string>> Mapping => new List<KeyValuePair<OrderType, string>>
        {
            new KeyValuePair<OrderType, string>(OrderType.MarketSell, "MARKET_SELL"),
            new KeyValuePair<OrderType, string>(OrderType.MarketBuy, "MARKET_BUY"),
            new KeyValuePair<OrderType, string>(OrderType.LimitSell, "LIMIT_SELL"),
            new KeyValuePair<OrderType, string>(OrderType.LimitBuy, "LIMIT_BUY"),
            new KeyValuePair<OrderType, string>(OrderType.UnknownType, "UNKNOWN_TYPE"),
            new KeyValuePair<OrderType, string>(OrderType.MarketBuyInFullAmount, "MARKET_BUY_IN_FULL_AMOUNT")
        };
    }
}
