using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Test
{
	[global::ProtoBuf.ProtoContract()]
	class TickerEvent
	{
        public DateTime Timestamp { get; set; }
        [global::ProtoBuf.ProtoMember(1)]
        long TimestampImpl { get => Timestamp.ToUnixMilliseconds(); set => Timestamp = value.ToUnixMilliseconds(); }

        [global::ProtoBuf.ProtoMember(2)]
        public string Last { get; set; } = string.Empty;

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue("")]
        public string High { get; set; } = string.Empty;
        [global::ProtoBuf.ProtoMember(4)]
        public string Low { get; set; } = string.Empty;
        [global::ProtoBuf.ProtoMember(5)]
        public string Volume { get; set; } = string.Empty;

        [global::ProtoBuf.ProtoMember(6)]
        public string Vwap { get; set; } = string.Empty;
        [global::ProtoBuf.ProtoMember(7)]
        public string MaxBid { get; set; } = string.Empty;
        [global::ProtoBuf.ProtoMember(8)]
        public string MinAsk { get; set; } = string.Empty;
        public decimal BestAsk { get; set; }
        public decimal BestBid { get; set; }
        [global::ProtoBuf.ProtoMember(9)]
        string BestBidImpl { get => BestBid.ToString(System.Globalization.CultureInfo.InvariantCulture); set => BestBid = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); } 
        [global::ProtoBuf.ProtoMember(10)]
        string BestAskImpl { get => BestAsk.ToString(System.Globalization.CultureInfo.InvariantCulture); set => BestAsk = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
    }
}
