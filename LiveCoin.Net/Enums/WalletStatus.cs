using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Enums
{
	/// <summary>
	/// status of a wallet
	/// </summary>
	public enum WalletStatus
	{
		/// <summary>
		///  Wallet online
		/// </summary>
		Normal,
		/// <summary>
		/// Wallet delayed (no new block for 1-2 hours)
		/// </summary>
		Delayed,
		/// <summary>
		/// Out of sync(no new block for at least 2 hours)
		/// </summary>
		Blocked,
		/// <summary>
		/// No new block for at least 24h(Out of sync)
		/// </summary>
		BlockedLong,
		/// <summary>
		/// Wallet temporary offline
		/// </summary>
		Down,
		/// <summary>
		/// Asset will be delisted soon, withdraw your funds.
		/// </summary>
		Delisted,
		/// <summary>
		/// Only withdrawal is available
		/// </summary>
		ClosedCashin,
		/// <summary>
		/// Only deposit is available
		/// </summary>
		ClosedCashout
	}
}
