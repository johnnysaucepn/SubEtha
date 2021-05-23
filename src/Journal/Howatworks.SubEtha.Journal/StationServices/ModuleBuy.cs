using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class ModuleBuy : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Slot { get; set; }
        public string BuyItem { get; set; }
        public string BuyItem_Localised { get; set; }
        public long BuyPrice { get; set; }
        public string Ship { get; set; } // Note: ship type
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long ShipID { get; set; }

        public string StoredItem { get; set; }

        public string SellItem { get; set; }
        public string SellItem_Localised { get; set; }
        public long? SellPrice { get; set; }
    }
}
