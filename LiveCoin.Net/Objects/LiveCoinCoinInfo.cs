using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Converters;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Public data for currencies
	/// </summary>
	public class LiveCoinCoinInfo
	{
		/// <summary>
		/// Minimal order in BTC
		/// </summary>
		[JsonProperty("minimalOrderBTC")]
		[JsonConverter(typeof(DecimalToStringConverter))]
		public decimal MinimalOrderBTC { get; set; }
		/// <summary>
		/// Public data for currencies
		/// </summary>
		[JsonProperty("info")]
		public IEnumerable<LiveCoinCoinInfoEntry> CoinInfos { get; set; } = new List<LiveCoinCoinInfoEntry>();

	}
}
