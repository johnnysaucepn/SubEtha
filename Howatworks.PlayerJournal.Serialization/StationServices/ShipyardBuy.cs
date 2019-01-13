using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class ShipyardBuy : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string ShipType { get; set; }
        public int ShipPrice { get; set; }
        public string StoreOldShip { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int StoreShipID { get; set; }
        public string SellOldShip { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int SellShipID { get; set; }
        public int SellPrice { get; set; }
    }
}
