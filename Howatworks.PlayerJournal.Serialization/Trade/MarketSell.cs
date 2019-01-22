using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Trade
{
    public class MarketSell : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Type { get; set; }
        public string Type_Localised { get; set; }
        public int Count { get; set; }
        public long SellPrice { get; set; }
        public long TotalSale { get; set; }
        public long AvgPricePaid { get; set; }
        public bool? IllegalGoods { get; set; }
        public bool? StolenGoods { get; set; }
        public bool? BlackMarket { get; set; }
    }
}
