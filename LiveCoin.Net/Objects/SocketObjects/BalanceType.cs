using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    /// <summary>
    /// Balance type
    /// </summary>
    public enum BalanceType
    {
        /// <summary>
        /// Total
        /// </summary>
        Total = 1,
        /// <summary>
        /// Available
        /// </summary>
        Available = 2,
        /// <summary>
        /// Available for withdraw
        /// </summary>
        AvailableWithdrawal = 3,
        /// <summary>
        /// Trade
        /// </summary>
        Trade = 4,
    }
}
