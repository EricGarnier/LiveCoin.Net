using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Enums
{
	/// <summary>
	/// Balance types
	/// </summary>
	public enum BalanceType
	{
		/// <summary>
		/// Total balance
		/// </summary>
		Total,
		/// <summary>
		/// Available balance
		/// </summary>
		Available,
		/// <summary>
		/// Trade balance
		/// </summary>
		Trade,
		/// <summary>
		/// Available for withdraw balance
		/// </summary>
		AvailableWithdrawal
	}
}
