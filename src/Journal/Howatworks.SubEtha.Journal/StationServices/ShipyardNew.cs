using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class ShipyardNew : JournalEntryBase
    {
        public string ShipType { get; set; }
        public long NewShipID { get; set; }
    }
}
