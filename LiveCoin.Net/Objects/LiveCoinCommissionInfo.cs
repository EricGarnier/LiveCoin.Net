using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Converters;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// actual trading fee and volume for the last 30 days in USD
	/// </summary>
	public class LiveCoinCommissionInfo
	{
		/// <summary>
		/// Commission for customer
		/// </summary>
		[JsonConverter(typeof(DecimalToStringConverter))]
		[JsonProperty("commission")]
		public decimal Commission { get; set; }
		/// <summary>
		/// Last 30 days amount as USD
		/// </summary>
		[JsonConverter(typeof(DecimalToStringConverter))]
		[JsonProperty("last30DaysAmountAsUSD")]
		public decimal Last30DaysAmountAsUSD { get; set; }
	}
}
