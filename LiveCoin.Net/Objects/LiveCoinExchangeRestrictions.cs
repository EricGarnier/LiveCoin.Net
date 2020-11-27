using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Restriction for all currency
	/// </summary>
	public class LiveCoinExchangeRestrictions
	{
		/// <summary>
		/// Minimum BTC volume
		/// </summary>
		[JsonProperty("minBtcVolume")]
		public decimal MinBtcVolume { get; set; }
		/// <summary>
		/// Minimum amount to open order, maximum number of digits after the decimal point in price value.
		/// </summary>
		[JsonProperty("restrictions")]
		public IEnumerable<LiveCoinRestriction> Restrictions { get; set; } = new List<LiveCoinRestriction>();
	}
}
