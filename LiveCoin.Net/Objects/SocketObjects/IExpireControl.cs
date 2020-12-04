using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
	internal interface IExpireControl
	{
		public RequestExpired? ExpireControl { get; set; }
	}
}
