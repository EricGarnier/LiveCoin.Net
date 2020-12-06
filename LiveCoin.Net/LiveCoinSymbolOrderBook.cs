using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;
using LiveCoin.Net.Objects.SocketObjects;

namespace LiveCoin.Net
{
	/// <summary>
	/// Implementation for a synchronized order book. After calling Start the order book will sync itself and keep up to date with new data. It will automatically try to reconnect and resync in case of a lost/interrupted connection.
	/// Make sure to check the State property to see if the order book is synced.
	/// </summary>
	public class LiveCoinSymbolOrderBook : SymbolOrderBook
	{
		private readonly bool _restClientOwner = false;
		private readonly LiveCoinClient _restClient;
		private readonly bool _socketClientOwner = false;
		private readonly LiveCoinSocketClient _socketClient;

		/// <summary>
		/// Create a new instance
		/// </summary>
		/// <param name="symbol">The symbol of the order book</param>
		/// <param name="options">The options for the order book</param>
		public LiveCoinSymbolOrderBook(string symbol, LiveCoinOrderBookOptions? options = null) : base(symbol, options ?? new LiveCoinOrderBookOptions())
		{
			symbol.ValidateLiveCoinSymbol();
			Levels = options?.Limit;
			if (options?.RestClient != null)
			{
				_restClient = options.RestClient;
			}
			else
			{
				_restClient = new LiveCoinClient();
				_restClientOwner = true;
			}
			if (options?.SocketClient != null)
			{
				_socketClient = options.SocketClient;
			}
			else
			{
				_socketClient = new LiveCoinSocketClient();
				_socketClientOwner = true;
			}
		}
		/// <inheritdoc />
		protected override async Task<CallResult<UpdateSubscription>> DoStart()
		{
			var subResult = await _socketClient.SubscribeOrderBookAsync(Symbol, Levels, HandleUpdate).ConfigureAwait(false);

			if (!subResult)
				return new CallResult<UpdateSubscription>(null, subResult.Error);

			Status = OrderBookStatus.Syncing;
			var bookResult = await _restClient.GetOrderBookAsync(Symbol, true, Levels).ConfigureAwait(false);
			if (!bookResult)
			{
				await _socketClient.Unsubscribe(subResult.Data).ConfigureAwait(false);
				return new CallResult<UpdateSubscription>(null, bookResult.Error);
			}

			SetInitialOrderBook(bookResult.Data.Time.ToUnixMilliseconds(), bookResult.Data.Bids, bookResult.Data.Asks);

			return new CallResult<UpdateSubscription>(subResult.Data, null);
		}

		private void HandleUpdate(OrderBookNotification data)
		{
				UpdateOrderBook(data.Data.Where(d => d.OrderType == OrderType.Bid), data.Data.Where(d => d.OrderType == OrderType.Ask));
		}

		/// <inheritdoc />
		protected override void DoReset()
		{
		}

		/// <inheritdoc />
		protected override async Task<CallResult<bool>> DoResync()
		{
			if (Levels != null)
				return await WaitForSetOrderBook(10000).ConfigureAwait(false);

			var bookResult = await _restClient.GetOrderBookAsync(Symbol, true, Levels).ConfigureAwait(false);
			if (!bookResult)
				return new CallResult<bool>(false, bookResult.Error);

			SetInitialOrderBook(bookResult.Data.Time.ToUnixMilliseconds(), bookResult.Data.Bids, bookResult.Data.Asks);
			return new CallResult<bool>(true, null);
		}

		/// <inheritdoc />
		public override void Dispose()
		{
			processBuffer.Clear();
			asks.Clear();
			bids.Clear();

			if (_restClientOwner) _restClient?.Dispose();
			if (_socketClientOwner) _socketClient?.Dispose();
		}

	}
}
