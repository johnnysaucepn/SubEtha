using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class ModuleBuy : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Slot { get; set; }
        public string BuyItem { get; set; }
        public string BuyItem_Localised { get; set; }
        public int BuyPrice { get; set; }
        public string Ship { get; set; } // Note: ship type
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }

        public string StoredItem { get; set; }

        public string SellItem { get; set; }
        public string SellItem_Localised { get; set; }
        public int? SellPrice { get; set; }
    }
}
