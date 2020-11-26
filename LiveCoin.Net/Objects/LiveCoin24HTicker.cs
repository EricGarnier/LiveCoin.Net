using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Price statistics of the last 24 hours
	/// </summary>
	public class LiveCoin24HTicker
	{
		/// <summary>
		/// Symbol. Only in allTickers
		/// </summary>
		[JsonProperty("symbol")]
		public string? Symbol { get; set; }
		/// <summary>
		/// Maximal bid for the last 24 hours
		/// </summary>
		[JsonProperty("max_bid")]
		public decimal MaxBidPrice { get; set; }
		/// <summary>
		/// Minimal ask for the last 24 hours
		/// </summary>
		[JsonProperty("min_ask")]
		public decimal MinBidPrice { get; set; }
		/// <summary>
		/// Best current bid 
		/// </summary>
		[JsonProperty("best_bid")]
		public decimal BidPrice { get; set; }
		/// <summary>
		/// Best current ask
		/// </summary>
		[JsonProperty("best_ask")]
		public decimal AskPrice { get; set; }
		/// <summary>
		/// Last price
		/// </summary>
		[JsonProperty("last")]
		public decimal LastPrice { get; set; }
		/// <summary>
		/// The higest price
		/// </summary>
		[JsonProperty("high")]
		public decimal HighPrice { get; set; }
		/// <summary>
		/// The lowest price
		/// </summary>
		[JsonProperty("low")]
		public decimal LowPrice { get; set; }
		/// <summary>
		/// The volume
		/// </summary>
		[JsonProperty("volume")]
		public decimal Volume { get; set; }
		/// <summary>
		/// The vwap
		/// </summary>
		[JsonProperty("vwap")]
		public decimal VWap { get; set; }

	}
}
