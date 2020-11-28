using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Order result
	/// </summary>
	public class LiveCoinOrderResult
	{
		/// <summary>
		/// Is the new order added
		/// </summary>
		[JsonProperty("added")]
		public bool Added { get; set; }
		/// <summary>
		/// The order Id
		/// </summary>
		[JsonProperty("orderId")]
		public long? OrderId { get; set; }
	}
}
