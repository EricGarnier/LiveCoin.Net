using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Cancel order result
	/// </summary>
	public class LiveCoinCancelResult
	{
		/// <summary>
		/// Is really cancelled
		/// </summary>
		[JsonProperty("cancelled")]
		public bool Cancelled { get; set; }
		/// <summary>
		/// Quantity
		/// </summary>
		[JsonProperty("quantity")]
		public decimal Quantity { get; set; }
		/// <summary>
		/// Trade quantity
		/// </summary>
		[JsonProperty("tradeQuantity")]
		public decimal TradeQuantity { get; set; }
		/// <summary>
		/// Exception during processing
		/// </summary>
		[JsonProperty("exception")]
		public string? Exception { get; set; }
	}
}
