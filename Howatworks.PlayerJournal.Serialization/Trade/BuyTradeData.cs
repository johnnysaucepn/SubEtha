namespace Howatworks.PlayerJournal.Serialization.Trade
{
    public class BuyTradeData : JournalEntryBase
    {
        public string System { get; set; } // Note: name
        public long Cost { get; set; }
    }
}
