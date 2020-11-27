using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Converters;
using LiveCoin.Net.Enums;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// exchange order information 
	/// </summary>
	public class LiveCoinExchangeOrder
	{
        /// <summary>
        /// Order Id
        /// </summary>
        [JsonProperty("id")]
        public long OrderId { get; set; }
        /// <summary>
        /// Client order Id
        /// </summary>
        [JsonProperty("client_id")]
        public long ClientId { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonProperty("status")]
        [JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string? Symbol { get; set; }
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
        [JsonProperty("remaining_quantity")]
        public decimal RemainingQuantity { get; set; }
        /// <summary>
        /// Blocked
        /// </summary>
        [JsonProperty("blocked")]
        public decimal Blocked { get; set; }
        /// <summary>
        /// Remaining blocked
        /// </summary>
        [JsonProperty("blocked_remain")]
        public decimal BlockedRemain { get; set; }
        /// <summary>
        /// Commission rate
        /// </summary>
        [JsonProperty("commission_rate")]
        public decimal CommissionRate { get; set; }
        /// <summary>
        /// Trades
        /// </summary>
        [JsonProperty("trades")]
        public LiveCoinExchangeTrades? Trades { get; set; }
    }
}
