using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Test
{
	[global::ProtoBuf.ProtoContract()]
	class WsRequestMetaData
	{
        [global::ProtoBuf.ProtoMember(1)]
        public WsRequestMsgType RequestType { get; set; } = WsRequestMsgType.SubscribeTicker;

        [global::ProtoBuf.ProtoMember(2)]
        public string Token { get; set; } = string.Empty;

        [global::ProtoBuf.ProtoMember(4)]
        public byte[]? Sign { get; set; }

        [global::ProtoBuf.ProtoContract()]
        public enum WsRequestMsgType
        {
            SubscribeTicker = 1,
            SubscribeOrderBookRaw = 2,
            SubscribeOrderBook = 3,
            SubscribeTrade = 4,
            SubscribeCandle = 5,
            Unsubscribe = 6,
            Login = 7,
            PutLimitOrder = 8,
            CancelLimitOrder = 9,
            Balance = 10,
            Balances = 11,
            LastTrades = 12,
            Trades = 13,
            ClientOrders = 14,
            ClientOrder = 15,
            Commission = 16,
            CommissionCommonInfo = 17,
            TradeHistory = 18,
            MarketOrder = 19,
            WalletAddress = 20,
            WithdrawalCoin = 21,
            WithdrawalPayeer = 22,
            WithdrawalCapitalist = 23,
            WithdrawalAdvcash = 24,
            PrivateSubscribeOrderRaw = 25,
            PrivateSubscribeTrade = 26,
            PrivateUnsubscribe = 27,
            WithdrawalYandex = 28,
            WithdrawalQiwi = 29,
            WithdrawalCard = 30,
            WithdrawalMastercard = 31,
            WithdrawalPerfectmoney = 32,
            VoucherMake = 33,
            VoucherAmount = 34,
            VoucherRedeem = 35,
            CancelOrders = 36,
            PingRequest = 37,
            SubscribeBalanceChange = 38,
        }
    }
}
