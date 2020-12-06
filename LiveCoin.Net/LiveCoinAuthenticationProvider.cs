using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;

namespace LiveCoin.Net
{
	internal class LiveCoinAuthenticationProvider : AuthenticationProvider
	{
		private readonly HMACSHA256 encryptor;

		public LiveCoinAuthenticationProvider(ApiCredentials apiCredentials) : base(apiCredentials)
		{
			if (apiCredentials.Secret == null)
				throw new ArgumentException("No valid API credentials provided. Key/Secret needed.");

			encryptor = new HMACSHA256(Encoding.ASCII.GetBytes(apiCredentials.Secret.GetString()));
		}

		public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
		{
			if (!signed)
				return new Dictionary<string, string>();
			if (Credentials.Key == null)
				throw new ArgumentException("No valid API credentials provided. Key/Secret needed.");

			var query = CreateParamOrderedString(parameters, true, arraySerialization);
			return new Dictionary<string, string> { { "Api-Key", Credentials.Key.GetString() } ,
				{"Sign",  ByteToString(encryptor.ComputeHash(Encoding.UTF8.GetBytes(query)))} };
		}

		public override byte[] Sign(byte[] toSign)
		{
			if (Credentials.Key == null)
				throw new ArgumentException("No valid API credentials provided. Key/Secret needed.");
			return encryptor.ComputeHash(toSign);
		}
		/// <summary>
		/// Create a query string of the specified parameters
		/// </summary>
		/// <param name="parameters">The parameters to use</param>
		/// <param name="urlEncodeValues">Whether or not the values should be url encoded</param>
		/// <param name="serializationType">How to serialize array parameters</param>
		/// <returns></returns>
		private static string CreateParamOrderedString(Dictionary<string, object> parameters, bool urlEncodeValues, ArrayParametersSerialization serializationType)
		{
			var uriString = "";
			foreach (var entry in parameters.OrderBy(kp => kp.Key).ToList())
			{
				if (entry.Value.GetType().IsArray)
				{
					if (serializationType == ArrayParametersSerialization.Array)
						uriString += $"{string.Join("&", ((object[])(urlEncodeValues ? Uri.EscapeDataString(entry.Value.ToString()) : entry.Value)).Select(v => $"{entry.Key}[]={v}"))}&";
					else
					{
						var array = (Array)entry.Value;
						uriString += string.Join("&", array.OfType<object>().Select(a => $"{entry.Key}={Uri.EscapeDataString(a.ToString())}"));
						uriString += "&";
					}
				}
				else
				{
					uriString += $"{entry.Key}={(urlEncodeValues ? Uri.EscapeDataString(entry.Value.ToString()) : entry.Value)}";
					uriString += "&";
				}
			}
			uriString = uriString.TrimEnd('&');
			return uriString;
		}
	}
}
