using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class ShipyardTransfer : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string ShipType { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
        public string System { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long ShipMarketID { get; set; }
        public decimal Distance { get; set; }
        public long TransferPrice { get; set; }
        public decimal TransferTime { get; set; } // TODO: check data type
    }
}
