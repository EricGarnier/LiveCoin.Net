using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Enums
{
	/// <summary>
	/// Filter by order type
	/// </summary>
	public enum OrderStatusFilter
	{
		/// <summary>
		/// All orders
		/// </summary>
		All,
		/// <summary>
		/// Open orders
		/// </summary>
		Open,
		/// <summary>
		/// Executed or cancelled orders
		/// </summary>
		Closed,
		/// <summary>
		/// Cancelled orders
		/// </summary>
		Cancelled,
		/// <summary>
		/// All orders except cancelled ones
		/// </summary>
		NotCancelled,
		/// <summary>
		/// partially filled orders
		/// </summary>
		Partially
	}
}
