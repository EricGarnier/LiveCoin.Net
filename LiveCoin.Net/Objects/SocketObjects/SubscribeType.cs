using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    /// <summary>
    /// Private order raw subscribe type
    /// </summary>
    public enum SubscribeType
    {
        /// <summary>
        /// Only event
        /// </summary>
        OnlyEvents = 1,
        /// <summary>
        /// With initial state
        /// </summary>
        WithInitialState = 2,
    }
}
