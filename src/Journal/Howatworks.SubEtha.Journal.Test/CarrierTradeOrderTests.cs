using Howatworks.SubEtha.Journal.FleetCarriers;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class CarrierTradeOrderTests
    {
        private const string PurchaseOrderSample = @"{
            ""timestamp"": ""2020-03-16T14:52:36Z"",
            ""event"": ""CarrierTradeOrder"",
            ""CarrierID"": 3700005632,
            ""BlackMarket"": false,
            ""Commodity"": ""mineraloil"",
            ""Commodity_Localised"": ""Mineral Oil"",
            ""PurchaseOrder"": 70,
            ""Price"": 228
            }";

        private const string SaleOrderSample = @"{
            ""timestamp"": ""2020-03-16T14:52:36Z"",
            ""event"": ""CarrierTradeOrder"",
            ""CarrierID"": 3700005633,
            ""BlackMarket"": true,
            ""Commodity"": ""mineraloil"",
            ""Commodity_Localised"": ""Mineral Oil"",
            ""SaleOrder"": 70,
            ""Price"": 229
            }";

        private const string CancelTradeSample = @"{
            ""timestamp"": ""2020-03-16T14:52:36Z"",
            ""event"": ""CarrierTradeOrder"",
            ""CarrierID"": 3700005634,
            ""BlackMarket"": false,
            ""Commodity"": ""mineraloil"",
            ""Commodity_Localised"": ""Mineral Oil"",
            ""CancelTrade"": true,
            ""Price"": 230
            }";

        [Fact]
        public void PurchaseOrder()
        {
            var purchase = JsonConvert.DeserializeObject<CarrierTradeOrder>(PurchaseOrderSample);
            Assert.Equal(3700005632, purchase.CarrierID);
            Assert.False(purchase.BlackMarket);
            Assert.Equal("mineraloil", purchase.Commodity);
            Assert.Equal("Mineral Oil", purchase.Commodity_Localised);
            Assert.Equal(70, purchase.PurchaseOrder);
            Assert.Null(purchase.SaleOrder);
            Assert.Null(purchase.CancelTrade);
            Assert.Equal(228, purchase.Price);
        }

        [Fact]
        public void SaleOrder()
        {
            var sale = JsonConvert.DeserializeObject<CarrierTradeOrder>(SaleOrderSample);
            Assert.Equal(3700005633, sale.CarrierID);
            Assert.True(sale.BlackMarket);
            Assert.Equal("mineraloil", sale.Commodity);
            Assert.Equal("Mineral Oil", sale.Commodity_Localised);
            Assert.Null(sale.PurchaseOrder);
            Assert.Equal(70, sale.SaleOrder);
            Assert.Null(sale.CancelTrade);
            Assert.Equal(229, sale.Price);
        }

        [Fact]
        public void CancelTrade()
        {
            var sale = JsonConvert.DeserializeObject<CarrierTradeOrder>(CancelTradeSample);
            Assert.Equal(3700005634, sale.CarrierID);
            Assert.False(sale.BlackMarket);
            Assert.Equal("mineraloil", sale.Commodity);
            Assert.Equal("Mineral Oil", sale.Commodity_Localised);
            Assert.Null(sale.PurchaseOrder);
            Assert.Null(sale.SaleOrder);
            Assert.True(sale.CancelTrade);
            Assert.Equal(230, sale.Price);
        }
    }
}
