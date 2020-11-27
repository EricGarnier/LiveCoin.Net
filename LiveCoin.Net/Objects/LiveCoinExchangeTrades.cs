using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Trades
	/// </summary>
	public class LiveCoinExchangeTrades
	{
		/// <summary>
		/// Number of trades
		/// </summary>
		[JsonProperty("trades")]
		public int Trades { get; set; }

		/// <summary>
		/// Amount
		/// </summary>
		[JsonProperty("amount")]
		public decimal Amount { get; set; }
		/// <summary>
		/// Quantity
		/// </summary>
		[JsonProperty("quantity")]
		public decimal Quantity { get; set; }
		/// <summary>
		/// Average price
		/// </summary>
		[JsonProperty("avg_price")]
		public decimal AvgPrice { get; set; }
		/// <summary>
		/// Commission
		/// </summary>
		[JsonProperty("commission")]
		public decimal Commission { get; set; }
		/// <summary>
		/// Bonus
		/// </summary>
		[JsonProperty("bonus")]
		public decimal Bonus { get; set; }
	}
}
