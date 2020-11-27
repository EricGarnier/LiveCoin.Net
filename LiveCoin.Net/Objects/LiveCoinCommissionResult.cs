using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Converters;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Actual trading fee for customer
	/// </summary>
	public class LiveCoinCommissionResult
	{
		/// <summary>
		/// Fee for customer
		/// </summary>
		[JsonConverter(typeof(DecimalToStringConverter))]
		[JsonProperty("fee")]
		public decimal Fee { get; set; }
	}
}
