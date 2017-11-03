namespace Howatworks.PlayerJournal.StationServices
{
    public class BuyDrones : JournalEntryBase
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public int BuyPrice { get; set; }
        public int TotalCost { get; set; }
    }
}
