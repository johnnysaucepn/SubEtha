using Howatworks.SubEtha.Journal.FleetCarriers;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class CarrierBankTransferTests
    {
        private const string DepositSample = @"{
            ""timestamp"": ""2020-03-24T15:34:46Z"",
            ""event"": ""CarrierBankTransfer"",
            ""CarrierID"": 3700005632,
            ""Deposit"": 80000,
            ""PlayerBalance"": 717339504128,
            ""CarrierBalance"": 3070010
            }";

        private const string WithdrawSample = @"{
            ""timestamp"": ""2020-03-24T15:34:46Z"",
            ""event"": ""CarrierBankTransfer"",
            ""CarrierID"": 3700005633,
            ""Withdraw"": 80000,
            ""PlayerBalance"": 717339604128,
            ""CarrierBalance"": 3020010
            }";

        [Fact]
        public void Deposit()
        {
            var deposit = JsonConvert.DeserializeObject<CarrierBankTransfer>(DepositSample);
            Assert.Equal(3700005632, deposit.CarrierID);
            Assert.Null(deposit.Withdraw);
            Assert.Equal(80000, deposit.Deposit);
            Assert.Equal(717339504128, deposit.PlayerBalance);
            Assert.Equal(3070010, deposit.CarrierBalance);
        }

        [Fact]
        public void Withdraw()
        {
            var withdraw = JsonConvert.DeserializeObject<CarrierBankTransfer>(WithdrawSample);
            Assert.Equal(3700005633, withdraw.CarrierID);
            Assert.Equal(80000, withdraw.Withdraw);
            Assert.Null(withdraw.Deposit);
            Assert.Equal(717339604128, withdraw.PlayerBalance);
            Assert.Equal(3020010, withdraw.CarrierBalance);
        }
    }
}
