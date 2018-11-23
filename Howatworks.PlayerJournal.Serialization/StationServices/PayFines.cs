namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class PayFines : JournalEntryBase
    {
        public int Amount { get; set; }
        public decimal BrokerPercentage { get; set; }
    }
}
