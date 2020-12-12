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
		Total = 1,
		/// <summary>
		/// Available balance
		/// </summary>
		Available = 2,
		/// <summary>
		/// Available for withdraw balance
		/// </summary>
		AvailableWithdrawal = 3,
		/// <summary>
		/// Trade balance
		/// </summary>
		Trade = 4,
	}
}
