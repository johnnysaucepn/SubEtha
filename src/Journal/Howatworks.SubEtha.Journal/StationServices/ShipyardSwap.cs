using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class ShipyardSwap : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string ShipType { get; set; }
        public string ShipType_Localised { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long ShipID { get; set; }
        public string StoreOldShip { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long StoreShipID { get; set; }
        public string SellOldShip { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long SellShipID { get; set; }
    }
}
