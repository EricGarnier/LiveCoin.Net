using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// The orderbook for a currency pair
	/// </summary>
	public class LiveCoinOrderBook
	{
		/// <summary>
		/// Orderbook timestamp
		/// </summary>
		[JsonProperty("timestamp")]
		[JsonConverter(typeof(TimestampConverter))]
		public DateTime Time { get; set; }
		/// <summary>
		/// Asks
		/// </summary>
		[JsonProperty("asks")]
		public IEnumerable<LiveCoinOrderBookEntry> Asks { get; set; } = new List<LiveCoinOrderBookEntry>();
		/// <summary>
		/// Bids
		/// </summary>
		[JsonProperty("bids")]

		public IEnumerable<LiveCoinOrderBookEntry> Bids { get; set; } = new List<LiveCoinOrderBookEntry>();

	}
}
