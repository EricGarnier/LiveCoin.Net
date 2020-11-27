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
	/// detailed order
	/// </summary>
	public class LiveCoinClientOrder
	{

        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }
        /// <summary>
        /// Identifier of currency pair
        /// </summary>
        [JsonProperty("currencyPair")]
        public string? Symbol{ get; set; }
        /// <summary>
        /// Good until time
        /// </summary>
        [JsonProperty("goodUntilTime")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime GoodUntilTime { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonProperty("orderStatus")]
        [JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus OrderStatus { get; set; }
        /// <summary>
        /// Issue yime
        /// </summary>
        [JsonProperty("issueTime")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime IssueTime { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Remaining quantity
        /// </summary>
        [JsonProperty("remainingQuantity")]
        public decimal RemainingQuantity { get; set; }
        /// <summary>
        /// Commission
        /// </summary>
        [JsonProperty("commission")]
        public string? Commission { get; set; }
        /// <summary>
        /// Commission rate
        /// </summary>
        [JsonProperty("commissionRate")]
        public decimal CommissionRate { get; set; }
        /// <summary>
        /// Last update
        /// </summary>
        [JsonProperty("lastModificationTime")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime LastModification{ get; set; }
    }
}
