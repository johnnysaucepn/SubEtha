using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Trade
{
    public class MarketBuy : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
        public long BuyPrice { get; set; }
        public long TotalCost { get; set; }
    }
}
