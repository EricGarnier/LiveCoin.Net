using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.Converters;
using LiveCoin.Net.Converters;
using LiveCoin.Net.Enums;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// A transaction
	/// </summary>
	public class LiveCoinTransaction
    {
        /// <summary>
        /// Transaction Id
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }
        /// <summary>
        /// Transaction type
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(TransactionTypeConverter))]
        public TransactionType TransactionType { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        [JsonProperty("date")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime Date { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonProperty("fee")]
        public decimal Fee{ get; set; }
        /// <summary>
        /// Fixed currency
        /// </summary>
        [JsonProperty("fixedCurrency")]
        public string? FixedCurrency { get; set; }
        /// <summary>
        /// Tax currency
        /// </summary>
        [JsonProperty("taxCurrency")]
        public string? TaxCurrency { get; set; }
        /// <summary>
        /// Variable Amount 
        /// </summary>
        [JsonProperty("variableAmount")]
        public decimal? VariableAmount { get; set; }
        /// <summary>
        /// Variable currency 
        /// </summary>
        [JsonProperty("variableCurrency")]
        public string? VariableCurrency { get; set; }
        /// <summary>
        /// External 
        /// </summary>
        [JsonProperty("external")]
        public string? External { get; set; }
        /// <summary>
        /// Login
        /// </summary>
        [JsonProperty("login")]
        public string? Login { get; set; }
    }
}
