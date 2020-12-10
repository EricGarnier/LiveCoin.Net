# ![Icon](https://github.com/EricGarnier/LiveCoin.Net/blob/master/LiveCoin.Net/Icon/icon.png?raw=true) LiveCoin.Net 

![Build status](https://travis-ci.org/EricGarnier/LiveCoin.Net.svg?branch=master)

A .Net wrapper for the LiveCoin API as described on [LiveCoin](https://www.livecoin.net/api), including all features the API provides.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/EricGarnier/LiveCoin.Net/issues)**

## CryptoExchange.Net
Implementation is build upon the CryptoExchange.Net library, make sure to also check out the documentation on that: [docs](https://github.com/JKorf/CryptoExchange.Net)

Other CryptoExchange.Net implementations:
<table>
<tr>
<td><a href="https://github.com/JKorf/Binance.Net"><img src="https://github.com/JKorf/Binance.Net/blob/master/Binance.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Binance.Net">Binance</a>
</td>
<td><a href="https://github.com/JKorf/Bittrex.Net"><img src="https://github.com/JKorf/Bittrex.Net/blob/master/Bittrex.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Bittrex.Net">Bittrex</a>
</td>
<td><a href="https://github.com/JKorf/Bitfinex.Net"><img src="https://github.com/JKorf/Bitfinex.Net/blob/master/Bitfinex.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Bitfinex.Net">Bitfinex</a>
</td>
<td><a href="https://github.com/JKorf/CoinEx.Net"><img src="https://github.com/JKorf/CoinEx.Net/blob/master/CoinEx.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/CoinEx.Net">CoinEx</a>
</td>
<td><a href="https://github.com/JKorf/Huobi.Net"><img src="https://github.com/JKorf/Huobi.Net/blob/master/Huobi.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Huobi.Net">Huobi</a>
</td>
<td><a href="https://github.com/JKorf/Kucoin.Net"><img src="https://github.com/JKorf/Kucoin.Net/blob/master/Kucoin.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Kucoin.Net">Kucoin</a>
</td>
<td><a href="https://github.com/JKorf/Kraken.Net"><img src="https://github.com/JKorf/Kraken.Net/blob/master/Kraken.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Kraken.Net">Kraken</a>
</td>
<td><a href="https://github.com/Zaliro/Switcheo.Net"><img src="https://github.com/Zaliro/Switcheo.Net/blob/master/Resources/switcheo-coin.png?raw=true"></a>
<br />
<a href="https://github.com/Zaliro/Switcheo.Net">Switcheo</a>
</td>
<td><a href="https://github.com/ridicoulous/LiquidQuoine.Net"><img src="https://github.com/ridicoulous/LiquidQuoine.Net/blob/master/Resources/icon.png?raw=true"></a>
<br />
<a href="https://github.com/ridicoulous/LiquidQuoine.Net">Liquid</a>
</td>
<td><a href="https://github.com/burakoner/OKEx.Net"><img src="https://raw.githubusercontent.com/burakoner/OKEx.Net/master/Okex.Net/Icon/icon.png"></a>
<br />
<a href="https://github.com/burakoner/OKEx.Net">OKEx</a>
</td>
	<td><a href="https://github.com/ridicoulous/Bitmex.Net"><img src="https://github.com/ridicoulous/Bitmex.Net/blob/master/Bitmex.Net/Icon/icon.png"></a>
<br />
<a href="https://github.com/ridicoulous/Bitmex.Net">Bitmex</a>
</td>
</tr>
</table>


## Installation
![Nuget version](https://img.shields.io/nuget/v/livecoin.net.svg)  ![Nuget downloads](https://img.shields.io/nuget/dt/LiveCoin.Net.svg)
Available on [Nuget](https://www.nuget.org/packages/LiveCoin.Net/).
```
pm> Install-Package LiveCoin.Net
```
To get started with LiveCoin.Net first you will need to get the library itself. The easiest way to do this is to install the package into your project using  [NuGet](https://www.nuget.org/packages/LiveCoin.Net/). Using Visual Studio this can be done in two ways.

### Using the package manager
In Visual Studio right click on your solution and select 'Manage NuGet Packages for solution...'. A screen will appear which initially shows the currently installed packages. In the top bit select 'Browse'. This will let you download net package from the NuGet server. In the search box type 'LiveCoin.Net' and hit enter. The LiveCoin.Net package should come up in the results. After selecting the package you can then on the right hand side select in which projects in your solution the package should install. After you've selected all project you wish to install and use LiveCoin.Net in hit 'Install' and the package will be downloaded and added to you projects.

### Using the package manager console
In Visual Studio in the top menu select 'Tools' -> 'NuGet Package Manager' -> 'Package Manager Console'. This should open up a command line interface. On top of the interface there is a dropdown menu where you can select the Default Project. This is the project that LiveCoin.Net will be installed in. After selecting the correct project type  `Install-Package LiveCoin.Net`  in the command line interface. This should install the latest version of the package in your project.

After doing either of above steps you should now be ready to actually start using LiveCoin.Net.
## Getting started
After installing it's time to actually use it. To get started we have to add the LiveCoin.Net namespace:  `using LiveCoin.Net;`.

LiveCoin.Net provides two clients to interact with the Binance API. The  `LiveCoinClient`  provides all rest API calls. The  `LiveCoinSocketClient`  provides functions to interact with the websocket provided by the LiveCoin API. Both clients are disposable and as such can be used in a  `using`statement.

## Examples
Examples can be found in the Examples folder.


## Release notes
* Version 1.0.4 - 10 dec 2020
    * Add ClientOrders and ClientOrder. Optimize signature process. Change Ping result.
* Version 1.0.3 - 08 dec 2020
    * Bug fix for private subscriptions.
* Version 1.0.2 - 08 dec 2020
    * Bug fix for private subscriptions.
* Version 1.0.1 - 07 dec 2020
    * Add CancelOrders on socket.
* Version 1.0.0 - 06 dec 2020
    * Add symbol order book, private subscriptions and correct some async signatures. Add also PutLimitOrder and CancelOrder on socket.
* Version 0.0.5 - 06 dec 2020
    * Add websocket with public subscriptions.
* Version 0.0.4 - 28 nov 2020
    * Only the REST API for Public data, Customer private data and open/cancel orders. Missing Rest deposit and withdrawal, vouchers. Also missing websockets.
