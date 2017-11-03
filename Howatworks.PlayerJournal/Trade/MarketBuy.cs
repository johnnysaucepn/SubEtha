namespace Howatworks.PlayerJournal.Trade
{
    public class MarketBuy : JournalEntryBase
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public int BuyPrice { get; set; }
        public int TotalCost { get; set; }
    }
}
