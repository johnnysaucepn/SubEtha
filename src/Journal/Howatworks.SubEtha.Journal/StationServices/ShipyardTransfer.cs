using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class ShipyardTransfer : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string ShipType { get; set; }
        public string ShipType_Localised { get; set; } // NOTE: not documented
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long ShipID { get; set; }
        public string System { get; set; } // NOTE: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long ShipMarketID { get; set; }
        public decimal Distance { get; set; }
        public long TransferPrice { get; set; }
        public decimal TransferTime { get; set; } // TODO: check data type
    }
}
