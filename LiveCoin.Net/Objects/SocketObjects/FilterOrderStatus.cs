using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    /// <summary>
    /// Filter order status
    /// </summary>
	public enum FilterOrderStatus
	{
        /// <summary>
        /// Open orders
        /// </summary>
        Open = 1,
        /// <summary>
        /// Closed orders
        /// </summary>
        Closed = 2,
        /// <summary>
        /// Cancelled orders
        /// </summary>
        Cancelled = 4,
        /// <summary>
        /// Partially filled orders
        /// </summary>
        Partially = 5,
        /// <summary>
        /// Not cancelled orders
        /// </summary>
        NotCancelled = 6,
    }
}
