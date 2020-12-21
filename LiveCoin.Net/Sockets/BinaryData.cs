using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using LiveCoin.Net.Objects.SocketObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LiveCoin.Net.Sockets
{
	internal class BinaryData
	{
		[JsonIgnore]
		private string? _data;
		[JsonIgnore]
		internal Func<string?>? _buildData;
		/// <summary>
		/// The binary data in base64 string
		/// </summary>
		public string? Data { get { if (_data == null && _buildData != null) { _data = _buildData(); } return _data; } }
		/// <summary>
		/// The token associate with the request
		/// </summary>
		[JsonIgnore]
		internal string? Token { get; set; }
		[JsonIgnore]
		internal WsResponseMsgType ExpectedResponseMsgType { get; set; }
		[JsonIgnore]
		internal Func<BinaryData, JToken, WsResponse, bool>? HandleMessageData;
		/// <summary>
		/// For subscription only
		/// </summary>
		[JsonIgnore]
		internal string? CurrencyPair { get; set; }
		/// <summary>
		/// For subscription only
		/// </summary>
		[JsonIgnore]
		internal ChannelType? ChannelType { get; set; }
		/// <summary>
		/// For subscription only
		/// </summary>
		[JsonIgnore]
		internal PrivateChannelType? PrivateChannelType { get; set; }
		[OnSerializing]
		internal void OnSerializingMethod(StreamingContext context)
		{
			_data = null;
		}

		public override string ToString()
		{
			return $"{nameof(BinaryData)}, token:'{Token}', ExpectedResponseMsgType:{ExpectedResponseMsgType}, currencyPair:{CurrencyPair}, channelType:{ChannelType}, privateChannelType:{PrivateChannelType}, data:{_data}";
		}
	}
}
