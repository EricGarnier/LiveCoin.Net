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
        /// New
        /// </summary>
        New = 1,
        /// <summary>
        /// Open
        /// </summary>
        Open = 2,
        /// <summary>
        /// Expired
        /// </summary>
        Expired = 3,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 4,
        /// <summary>
        /// Executed
        /// </summary>
        Executed = 5,
        /// <summary>
        /// Partially filled
        /// </summary>
        PartiallyFilled = 6,
        /// <summary>
        /// Partially filled and cancelled
        /// </summary>
        PartiallyFilledAndCancelled = 7,
        /// <summary>
        /// Partially filled and expired
        /// </summary>
        PartiallyFilledAndExpired = 8,
        /// <summary>
        /// Unknown
        /// </summary>
        UnknownStatus = 9,
    }
}
