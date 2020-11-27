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
	/// Detailed on a client transactions
	/// </summary>
	public class LiveCoinClientTrade
	{
		/// <summary>
		/// Transaction time
		/// </summary>
		[JsonProperty("datetime")]
		[JsonConverter(typeof(TimestampSecondsConverter))]
		public DateTime Timestamp { get; set; }
		/// <summary>
		/// Transaction Id
		/// </summary>
		[JsonProperty("id")]
		public long Id { get; set; }
		/// <summary>
		/// Trading symbol of cryptocurrency
		/// </summary>
		[JsonProperty("symbol")]
		public string? Symbol { get; set; }
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
		/// Transaction commission
		/// </summary>
		[JsonProperty("commission")]
		public decimal Commission { get; set; }
		/// <summary>
		/// Side of the trade
		/// </summary>
		[JsonProperty("type")]
		[JsonConverter(typeof(SideConverter))]
		public Side Side { get; set; }
		/// <summary>
		/// Client order id
		/// </summary>
		[JsonProperty("clientorderid")]
		public long ClientOrderId { get; set; }
	}
}
