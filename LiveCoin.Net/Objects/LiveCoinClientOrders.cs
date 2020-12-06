using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LiveCoin.Net.Objects
{
	/// <summary>
	/// Detailed review of your orders
	/// </summary>
	public class LiveCoinClientOrders
	{
        /// <summary>
        /// number of records
        /// </summary>
        [JsonProperty("totalRows")]
        public int TotalRows { get; set; }
        /// <summary>
        /// Sequence number of the first record
        /// </summary>
        [JsonProperty("startRow")]
        public int StartRow { get; set; }
        /// <summary>
        /// Sequence number of the last record
        /// </summary>
        [JsonProperty("endRow")]
        public int EndRow { get; set; }
        /// <summary>
        /// Detailed orders
        /// </summary>
        [JsonProperty("data")]
        public IEnumerable<LiveCoinClientOrder> Orders { get;} = new List<LiveCoinClientOrder>();
    }
}
