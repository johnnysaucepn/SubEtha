using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Trade
{
    public class MarketSell : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
        public int SellPrice { get; set; }
        public int TotalSale { get; set; }
        public int AvgPricePaid { get; set; } // TODO: check data type
        public bool? IllegalGoods { get; set; }
        public bool? StolenGoods { get; set; }
        public bool? BlackMarket { get; set; }
    }
}
