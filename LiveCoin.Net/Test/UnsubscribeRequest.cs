using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Test
{
	[global::ProtoBuf.ProtoContract()]
	class UnsubscribeRequest
	{
        [global::ProtoBuf.ProtoMember(1)]
        public ChannelType Channel_Type { get; set; } = ChannelType.Ticker;

        [global::ProtoBuf.ProtoMember(2)]
        public string CurrencyPair { get; set; }

        public enum ChannelType
        {
            Ticker = 1,
            OrderBookRaw = 2,
            OrderBook = 3,
            Trade = 4,
            Candle = 5,
        }
    }
}
