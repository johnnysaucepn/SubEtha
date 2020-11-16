using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class ShipyardBuy : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string ShipType { get; set; }
        public long ShipPrice { get; set; }
        public string StoreOldShip { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int StoreShipID { get; set; }
        public string SellOldShip { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int SellShipID { get; set; }
        public long SellPrice { get; set; }
    }
}
