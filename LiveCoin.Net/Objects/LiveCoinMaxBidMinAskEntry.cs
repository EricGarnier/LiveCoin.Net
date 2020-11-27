using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Converters;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Maximun bid and minimum ask for a symbol
	/// </summary>
	public class LiveCoinMaxBidMinAskEntry
	{
		/// <summary>
		/// Symbol.
		/// </summary>
		[JsonProperty("symbol")]
		public string? Symbol { get; set; }
		/// <summary>
		/// Maximum bid 
		/// </summary>
		[JsonProperty("maxBid")]
		[JsonConverter(typeof(DecimalToStringConverter))]
		public decimal MaxBid { get; set; }
		/// <summary>
		/// Minimum ask 
		/// </summary>
		[JsonProperty("minAsk")]
		[JsonConverter(typeof(DecimalToStringConverter))]
		public decimal MinAsk { get; set; }
	}
}
