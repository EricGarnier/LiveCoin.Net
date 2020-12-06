using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    /// <summary>
    /// A candle event
    /// </summary>
    [global::ProtoBuf.ProtoContract()]
    public class CandleEvent
	{
        /// <summary>
        /// timestamp of interval start
        /// </summary>
        public DateTime Timestamp { get; set; }
        [global::ProtoBuf.ProtoMember(1)]
        private long TimestampImpl { get => Timestamp.ToUnixMilliseconds(); set => Timestamp = value.ToUnixMilliseconds(); }

        /// <summary>
        /// Open price
        /// </summary>
        public decimal OpenPrice { get; set; }
        [global::ProtoBuf.ProtoMember(2)]
        private string OpenPriceImpl { get => OpenPrice.ToString(System.Globalization.CultureInfo.InvariantCulture); set => OpenPrice = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }

        /// <summary>
        /// Close price
        /// </summary>
        public decimal ClosePrice { get; set; }
        [global::ProtoBuf.ProtoMember(3)]
        private string ClosePriceImpl { get => ClosePrice.ToString(System.Globalization.CultureInfo.InvariantCulture); set => ClosePrice = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// High price
        /// </summary>
        public decimal HighPrice { get; set; }
        [global::ProtoBuf.ProtoMember(4)]
        private string HighPriceImpl { get => HighPrice.ToString(System.Globalization.CultureInfo.InvariantCulture); set => HighPrice = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Low price
        /// </summary>
        public decimal LowPrice { get; set; }
        [global::ProtoBuf.ProtoMember(5)]
        private string LowPriceImpl { get => LowPrice.ToString(System.Globalization.CultureInfo.InvariantCulture); set => LowPrice = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
        /// <summary>
        /// Volume
        /// </summary>
        public decimal Volume { get; set; }
        [global::ProtoBuf.ProtoMember(6)]
        private string VolumeImpl { get => Volume.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Volume = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }

        /// <summary>
        /// Quoted volume
        /// </summary>
        public decimal QuotedVolume { get; set; }
        [global::ProtoBuf.ProtoMember(7)]
        private string QuotedVolumeImpl { get => QuotedVolume.ToString(System.Globalization.CultureInfo.InvariantCulture); set => QuotedVolume = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
    }
}
