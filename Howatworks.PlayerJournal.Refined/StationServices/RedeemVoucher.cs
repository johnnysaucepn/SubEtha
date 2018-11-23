namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class RedeemVoucher : JournalEntryBase
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public decimal BrokerPercentage { get; set; }
    }
}
