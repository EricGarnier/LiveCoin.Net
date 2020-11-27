using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Converters;
using LiveCoin.Net.Enums;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Element of balance
	/// </summary>
	public class LiveCoinBalanceElement
	{
		/// <summary>
		/// Balance type
		/// </summary>
		[JsonProperty("type"), JsonConverter(typeof(BalanceTypeConverter))]
		public BalanceType BalanceType { get; set; }
		/// <summary>
		/// Currency code
		/// </summary>
		[JsonProperty("currency")]
		public string? Asset { get; set; }
		/// <summary>
		/// Amaount
		/// </summary>
		[JsonProperty("value")]
		public decimal Amount { get; set; }
	}
}
