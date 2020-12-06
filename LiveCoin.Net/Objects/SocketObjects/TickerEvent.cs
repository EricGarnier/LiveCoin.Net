using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// Ticker event
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class TickerEvent
	{
		/// <summary>
		/// Timestamp
		/// </summary>
		public DateTime Timestamp { get; set; }
		[global::ProtoBuf.ProtoMember(1)]
		private long TimestampImpl { get => Timestamp.ToUnixMilliseconds(); set => Timestamp = value.ToUnixMilliseconds(); }
		/// <summary>
		/// Last
		/// </summary>
		public decimal Last { get; set; }
		[global::ProtoBuf.ProtoMember(2)]
		private string LastImpl { get => Last.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Last = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// High
		/// </summary>
		public decimal High { get; set; }
		[global::ProtoBuf.ProtoMember(3)]
		private string HighImpl { get => High.ToString(System.Globalization.CultureInfo.InvariantCulture); set => High = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Low
		/// </summary>
		public decimal Low { get; set; }
		[global::ProtoBuf.ProtoMember(4)]
		private string LowImpl { get => Low.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Low = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Volume
		/// </summary>
		public decimal Volume { get; set; }
		[global::ProtoBuf.ProtoMember(5)]
		private string VolumeImpl { get => Volume.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Volume = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// VWap
		/// </summary>
		public decimal Vwap { get; set; }
		[global::ProtoBuf.ProtoMember(6)]
		private string VwapImpl { get => Vwap.ToString(System.Globalization.CultureInfo.InvariantCulture); set => Vwap = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Max bid
		/// </summary>
		public decimal MaxBid { get; set; }
		[global::ProtoBuf.ProtoMember(7)]
		private string MaxBidImpl { get => MaxBid.ToString(System.Globalization.CultureInfo.InvariantCulture); set => MaxBid = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Lin bid
		/// </summary>
		public decimal MinAsk { get; set; }
		[global::ProtoBuf.ProtoMember(8)]
		private string MinAskImpl { get => MinAsk.ToString(System.Globalization.CultureInfo.InvariantCulture); set => MinAsk = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Best bid
		/// </summary>
		public decimal BestBid { get; set; }
		[global::ProtoBuf.ProtoMember(9)]
		private string BestBidImpl { get => BestBid.ToString(System.Globalization.CultureInfo.InvariantCulture); set => BestBid = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
		/// <summary>
		/// Best ask
		/// </summary>
		public decimal BestAsk { get; set; }
		[global::ProtoBuf.ProtoMember(10)]
		private string BestAskImpl { get => BestAsk.ToString(System.Globalization.CultureInfo.InvariantCulture); set => BestAsk = decimal.Parse(value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); }
	}
}
