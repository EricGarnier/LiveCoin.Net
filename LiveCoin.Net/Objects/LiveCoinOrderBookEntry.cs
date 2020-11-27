using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Converters;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// An entry in the order book
	/// </summary>
	[JsonConverter(typeof(ArrayConverter))]
	public class LiveCoinOrderBookEntry
	{
		/// <summary>
		/// The price of this order book entry
		/// </summary>
		[ArrayProperty(0)]
		[JsonConverter(typeof(DecimalToStringConverter))]
		public decimal Price { get; set; }
		/// <summary>
		/// The quantity of this price in the order book
		/// </summary>
		[ArrayProperty(1)]
		[JsonConverter(typeof(DecimalToStringConverter))]
		public decimal Quantity { get; set; }
	}
}
