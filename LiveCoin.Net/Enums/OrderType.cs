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
		/// Market sell
		/// </summary>
		MarketSell,
		/// <summary>
		/// Market buy
		/// </summary>
		MarketBuy,
		/// <summary>
		/// Limit sell
		/// </summary>
		LimitSell,
		/// <summary>
		/// Limit sell
		/// </summary>
		LimitBuy,
		/// <summary>
		/// Unknown type
		/// </summary>
		UnknownType,
		/// <summary>
		/// Market buy in full amount
		/// </summary>
		MarketBuyInFullAmount,
	}
}
