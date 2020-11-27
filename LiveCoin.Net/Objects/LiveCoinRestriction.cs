using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	///  minimum amount to open order, for each pair and maximum number of digits after the decimal point in price value.
	/// </summary>
	public class LiveCoinRestriction
	{
		/// <summary>
		/// Symbol.
		/// </summary>
		[JsonProperty("currencyPair")]
		public string? Symbol { get; set; }
		/// <summary>
		/// maximum number of digits after the decimal point in price value
		/// </summary>
		[JsonProperty("priceScale")]
		public int PriceScale { get; set; }
		/// <summary>
		/// minimum amount to open order
		/// </summary>
		[JsonProperty("minLimitQuantity")]
		public decimal MinimumLimitQuantity { get; set; }
	}
}
