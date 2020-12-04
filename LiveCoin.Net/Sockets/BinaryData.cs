using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Test;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LiveCoin.Net.Sockets
{
	internal class BinaryData
	{
		/// <summary>
		/// The binary data in base64 string
		/// </summary>
		public string? Data { get; set; }
		/// <summary>
		/// The token associate with the request
		/// </summary>
		[JsonIgnore]
		internal string? Token { get; set; }
		[JsonIgnore]
		internal WsResponseMetaData.WsResponseMsgType ExpectedResponseMsgType { get; set; }
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
		internal UnsubscribeRequest.ChannelType? ChannelType { get; set; }
	}
}
