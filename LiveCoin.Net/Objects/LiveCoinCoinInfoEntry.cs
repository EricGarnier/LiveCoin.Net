using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Converters;
using LiveCoin.Net.Enums;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// public data for a currency
	/// </summary>
	public class LiveCoinCoinInfoEntry
	{
		/// <summary>
		/// Name of cryptocurrency
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
		/// <summary>
		/// Trading symbol of cryptocurrency
		/// </summary>
		[JsonProperty("symbol")]
		public string? Symbol { get; set; }
		/// <summary>
		/// Actual status of the wallet
		/// </summary>
		[JsonProperty("walletStatus")]
		[JsonConverter(typeof(WalletStatusConverter))]
		public WalletStatus WalletStatus { get; set; }
		/// <summary>
		/// Fee for outgoing transactions
		/// </summary>
		[JsonProperty("withdrawFee")]
		public decimal WithdrawFee { get; set; }
		/// <summary>
		/// Minimum amount for deposit
		/// </summary>
		[JsonProperty("minDepositAmount")]
		public decimal MinDepositAmount { get; set; }
		/// <summary>
		/// Minimum amount for withdrawal
		/// </summary>
		[JsonProperty("minWithdrawAmount")]
		public decimal MinWithdrawAmount { get; set; }
	}
}
