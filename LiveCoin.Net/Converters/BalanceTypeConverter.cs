using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Enums;

namespace LiveCoin.Net.Converters
{
	internal class BalanceTypeConverter : BaseConverter<BalanceType>
    {
        public BalanceTypeConverter() : this(true) { }
        public BalanceTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BalanceType, string>> Mapping => new List<KeyValuePair<BalanceType, string>>
        {
            new KeyValuePair<BalanceType, string>(BalanceType.Total, "total"),
            new KeyValuePair<BalanceType, string>(BalanceType.Available, "available"),
            new KeyValuePair<BalanceType, string>(BalanceType.AvailableWithdrawal, "available_withdrawal"),
            new KeyValuePair<BalanceType, string>(BalanceType.Trade, "trade")
        };
    }
}
