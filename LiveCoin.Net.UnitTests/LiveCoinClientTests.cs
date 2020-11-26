using System;
using System.Collections.Generic;
using System.Text;
using LiveCoin.Net.Objects;
using LiveCoin.Net.UnitTests.TestImplementations;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LiveCoin.Net.UnitTests
{
	[TestFixture()]
	public class LiveCoinClientTests
	{
        [TestCase]
        public void Get24HTicker_Should_RespondWithPricesForSymbol()
        {
            // arrange
            var expected = new LiveCoin24HTicker()
            {
                AskPrice = 0.123m,
                BidPrice = 0.456m,
                HighPrice = 0.789m,
                LastPrice = 1.123m,
                LowPrice = 1.456m,
                Volume = 3.123m,
                Symbol = "CBC/ETH",
                MaxBidPrice = 4.123m,
                MinBidPrice = 2.123m,
                VWap = 1024m
            };

            var client = TestHelpers.CreateResponseClient(JsonConvert.SerializeObject(expected));

            // act
            var result = client.Get24HTicker("CBC/ETH");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }
    }
}
