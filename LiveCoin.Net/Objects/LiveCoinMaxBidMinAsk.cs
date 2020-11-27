using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Maximun bid and minimum ask
	/// </summary>

	public class LiveCoinMaxBidMinAsk
	{
		/// <summary>
		/// All max bid and min ask per currency pair
		/// </summary>
		public LiveCoinMaxBidMinAskEntry[]? CurrencyPairs { get; set; }
	}
}
