using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// deposit address
	/// </summary>
	public class LiveCoinAddress
	{
		/// <summary>
		/// User id
		/// </summary>
		[JsonProperty("userId")]
		public long UserId { get; set; }
		/// <summary>
		/// User name
		/// </summary>
		[JsonProperty("userName")]
		public string? UserName { get; set; }
		/// <summary>
		/// Currency
		/// </summary>
		[JsonProperty("currency")]
		public string? Currency { get; set; }
		/// <summary>
		/// Wallet
		/// Wallet field has a delimiter "::" for using if you need Memo or Payment ID data besides your wallet to deposit coins (coins like: XMR, BTS, THS, STEEM). In this case wallet data is entered before delimiter, and Memo/Payment ID - after.
		/// </summary>
		[JsonProperty("wallet")]
		public string? Wallet { get; set; }

	}
}
