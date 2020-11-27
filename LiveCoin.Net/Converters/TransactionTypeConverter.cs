using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Enums;

namespace LiveCoin.Net.Converters
{
	internal class TransactionTypeConverter : BaseConverter<TransactionType>
    {
        public TransactionTypeConverter() : this(true) { }
        public TransactionTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<TransactionType, string>> Mapping => new List<KeyValuePair<TransactionType, string>>
        {
            new KeyValuePair<TransactionType, string>(TransactionType.Buy, "BUY"),
            new KeyValuePair<TransactionType, string>(TransactionType.Sell, "SELL"),
            new KeyValuePair<TransactionType, string>(TransactionType.Deposit, "DEPOSIT"),
            new KeyValuePair<TransactionType, string>(TransactionType.Withdrawal, "WITHDRAWAL")
        };
    }
}
