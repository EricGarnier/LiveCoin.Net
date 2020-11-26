using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LiveCoin.Net
{
    /// <summary>
    /// Helper methods for the LiveCoin API
    /// </summary>
    public static class LiveCoinHelpers
    {
        /// <summary>
        /// Validate the string is a valid LiveCoin symbol.
        /// </summary>
        /// <param name="symbolString">string to validate</param>
        public static void ValidateLiveCoinSymbol(this string symbolString)
        {
            if (string.IsNullOrEmpty(symbolString))
                throw new ArgumentException("Symbol is not provided");

            if (!Regex.IsMatch(symbolString, "^([A-Za-z]{3,})/([A-Za-z]{3,})$"))
                throw new ArgumentException($"{symbolString} is not a valid LiveCoin symbol. Should be [BaseCurrency]/[QuoteCurrency], e.g. ETH/BTC");
        }
    }
}
