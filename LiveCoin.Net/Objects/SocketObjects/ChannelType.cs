using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    internal enum ChannelType
    {
        Ticker = 1,
        OrderBookRaw = 2,
        OrderBook = 3,
        Trade = 4,
        Candle = 5,
    }
}
