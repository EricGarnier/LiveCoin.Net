using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Enums
{
	/// <summary>
	/// Order type
	/// </summary>
	public enum OrderType
	{
        /// <summary>
        /// Market buy
        /// </summary>
        MarketBuy = 1,
        /// <summary>
        /// Market sell
        /// </summary>
        MarketSell = 2,
        /// <summary>
        /// Limit buy
        /// </summary>
        LimitBuy = 3,
        /// <summary>
        /// Limit sell
        /// </summary>
        LimitSell = 4,
        /// <summary>
        /// Unknown
        /// </summary>
        UnknownType = 5,
        /// <summary>
        /// Market buy in full amount
        /// </summary>
        MarketBuyInFullAmount = 6,
    }
}
