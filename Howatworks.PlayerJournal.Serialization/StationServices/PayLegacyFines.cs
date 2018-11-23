namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class PayLegacyFines : JournalEntryBase
    {
        public int Amount { get; set; }
        public decimal BrokerPercentage { get; set; }
    }
}
