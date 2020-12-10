using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	/// <summary>
	/// List of orders
	/// </summary>
	[global::ProtoBuf.ProtoContract()]
	public class ClientOrdersResponse
	{
        /// <summary>
        /// Total rows
        /// </summary>
        [global::ProtoBuf.ProtoMember(1)]
        public int TotalRows { get; set; }
        /// <summary>
        /// Start row
        /// </summary>
        [global::ProtoBuf.ProtoMember(2)]
        public int StartRow { get; set; }
        /// <summary>
        /// End row
        /// </summary>
        [global::ProtoBuf.ProtoMember(3)]
        public int EndRow { get; set; }
        /// <summary>
        /// Orders
        /// </summary>
        [global::ProtoBuf.ProtoMember(4)]
        public global::System.Collections.Generic.List<Order> Orders { get; } = new global::System.Collections.Generic.List<Order>();

    }
}
