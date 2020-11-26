using System;
using System.Collections.Generic;
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
		private readonly ArrayParametersSerialization arraySerialization;

		public LiveCoinAuthenticationProvider(ApiCredentials apiCredentials, ArrayParametersSerialization multipleValues) : base(apiCredentials)
		{
			if (apiCredentials.Secret == null)
				throw new ArgumentException("No valid API credentials provided. Key/Secret needed.");

			encryptor = new HMACSHA256(Encoding.ASCII.GetBytes(apiCredentials.Secret.GetString()));
			this.arraySerialization = multipleValues;
		}

		public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
		{
			if (!signed)
				return new Dictionary<string, string>();
			if (Credentials.Key == null)
				throw new ArgumentException("No valid API credentials provided. Key/Secret needed.");

			var query = parameters.CreateParamString(true, arraySerialization);
			return new Dictionary<string, string> { { "Api-Key", Credentials.Key.GetString() } ,
				{"Sign",  ByteToString(encryptor.ComputeHash(Encoding.UTF8.GetBytes(query)))} };
		}

		public override string Sign(string toSign)
		{
			throw new NotImplementedException();
		}
	}
}
