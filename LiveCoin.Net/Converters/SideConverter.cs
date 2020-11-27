using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Enums;

namespace LiveCoin.Net.Converters
{
    internal class SideConverter : BaseConverter<Side>
    {
        public SideConverter() : this(true) { }
        public SideConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<Side, string>> Mapping => new List<KeyValuePair<Side, string>>
        {
            new KeyValuePair<Side, string>(Side.Buy, "BUY"),
            new KeyValuePair<Side, string>(Side.Sell, "SELL")
        };
    }
}
