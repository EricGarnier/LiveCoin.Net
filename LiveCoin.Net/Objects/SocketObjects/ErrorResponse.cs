﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Objects.SocketObjects
{
    [global::ProtoBuf.ProtoContract()]
    internal class ErrorResponse
	{
        [global::ProtoBuf.ProtoMember(1)]
        public int Code { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public string? Message { get; set; }

    }
}
