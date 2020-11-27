using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Converters;
using LiveCoin.Net.Enums;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Detailed on a transactions for a currency pair
	/// </summary>
	public class LiveCoinTrade
	{
		/// <summary>
		/// Transaction time
		/// </summary>
		[JsonProperty("time")]
		[JsonConverter(typeof(TimestampSecondsConverter))]
		public DateTime Timestamp { get; set; }
		/// <summary>
		/// Transaction Id
		/// </summary>
		[JsonProperty("id")]
		public long Id { get; set; }
		/// <summary>
		/// Transaction price
		/// </summary>
		[JsonProperty("price")]
		public decimal Price { get; set; }
		/// <summary>
		/// Transaction quantity
		/// </summary>
		[JsonProperty("quantity")]
		public decimal Quantity { get; set; }
		/// <summary>
		/// Side of the trade
		/// </summary>
		[JsonProperty("type")]
		[JsonConverter(typeof(SideConverter))]
		public Side Side { get; set; }
	}
}
