using System;
using System.Collections.Generic;
using System.Text;

namespace LiveCoin.Net.Test
{
	[global::ProtoBuf.ProtoContract()]
	class WsResponseMetaData
	{
        [global::ProtoBuf.ProtoMember(1)]
        public WsResponseMsgType ResponseType { get; set; } = WsResponseMsgType.TickerChannelSubscribed;

        [global::ProtoBuf.ProtoMember(2)]
        public string Token { get; set; } = string.Empty;
        [global::ProtoBuf.ProtoContract()]
        public enum WsResponseMsgType
        {
            TickerChannelSubscribed = 1,
            OrderBookRawChannelSubscribed = 2,
            OrderBookChannelSubscribed = 3,
            TradeChannelSubscribed = 4,
            CandleChannelSubscribed = 5,
            ChannelUnsubscribed = 6,
            Error = 7,
            TickerNotify = 8,
            OrderBookRawNotify = 9,
            OrderBookNotify = 10,
            TradeNotify = 11,
            CandleNotify = 12,
            LoginResponse = 13,
            PutLimitOrderResponse = 14,
            CancelLimitOrderResponse = 15,
            BalanceResponse = 16,
            BalancesResponse = 17,
            LastTradesResponse = 18,
            TradesResponse = 19,
            ClientOrdersResponse = 20,
            ClientOrderResponse = 21,
            CommissionResponse = 22,
            CommissionCommonInfoResponse = 23,
            TradeHistoryResponse = 24,
            MarketOrderResponse = 25,
            WalletAddressResponse = 26,
            WithdrawalCoinResponse = 27,
            WithdrawalPayeerResponse = 28,
            WithdrawalCapitalistResponse = 29,
            WithdrawalAdvcashResponse = 30,
            PrivateOrderRawChannelSubscribed = 31,
            PrivateTradeChannelSubscribed = 32,
            PrivateOrderRawNotify = 33,
            PrivateTradeNotify = 34,
            PrivateChannelUnsubscribed = 35,
            WithdrawalYandexResponse = 36,
            WithdrawalQiwiResponse = 37,
            WithdrawalCardResponse = 38,
            WithdrawalMastercardResponse = 39,
            WithdrawalPerfectmoneyResponse = 40,
            VoucherMakeResponse = 41,
            VoucherAmountResponse = 42,
            VoucherRedeemResponse = 43,
            CancelOrdersResponse = 44,
            PongResponse = 45,
            BalanceChangeChannelSubscribed = 46,
            BalanceChangeNotify = 47,
        }
    }
}
