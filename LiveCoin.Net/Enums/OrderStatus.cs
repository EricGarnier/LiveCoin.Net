using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Enums
{
	/// <summary>
	/// Order status
	/// </summary>
	public enum OrderStatus
	{
		/// <summary>
		/// New order
		/// </summary>
		New,
		/// <summary>
		/// Open
		/// </summary>
		Open,
		/// <summary>
		/// Expired
		/// </summary>
		Expired,
		/// <summary>
		/// Cancelled
		/// </summary>
		Cancelled,
		/// <summary>
		/// Executed
		/// </summary>
		Executed,
		/// <summary>
		/// Partially filled
		/// </summary>
		PartiallyFilled,
		/// <summary>
		/// Partially filled and cancelled
		/// </summary>
		PartiallyFilledAndCancelled,
		/// <summary>
		/// Partially filled and expired
		/// </summary>
		PartiallyFilledAndExpired,
		/// <summary>
		/// Unknown status
		/// </summary>
		UnknownStatus
	}
}
