using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Enums;

namespace LiveCoin.Net.Converters
{
	internal class WalletStatusConverter : BaseConverter<WalletStatus>
    {
        public WalletStatusConverter() : this(true) { }
        public WalletStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<WalletStatus, string>> Mapping => new List<KeyValuePair<WalletStatus, string>>
        {
            new KeyValuePair<WalletStatus, string>(WalletStatus.Normal, "normal"),
            new KeyValuePair<WalletStatus, string>(WalletStatus.Delayed, "delayed"),
            new KeyValuePair<WalletStatus, string>(WalletStatus.Blocked, "blocked"),
            new KeyValuePair<WalletStatus, string>(WalletStatus.BlockedLong, "blocked_long"),
            new KeyValuePair<WalletStatus, string>(WalletStatus.Down, "down"),
            new KeyValuePair<WalletStatus, string>(WalletStatus.Delisted, "delisted"),
            new KeyValuePair<WalletStatus, string>(WalletStatus.ClosedCashin, "closed_cashin"),
            new KeyValuePair<WalletStatus, string>(WalletStatus.ClosedCashout, "closed_cashout")
        };
    }
}
