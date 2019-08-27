using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class ShipyardSell : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string ShipType { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int SellShipID { get; set; }
        public long ShipPrice { get; set; }
        public string System { get; set; }
    }
}
