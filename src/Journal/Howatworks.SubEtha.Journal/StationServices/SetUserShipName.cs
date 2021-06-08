using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class SetUserShipName : JournalEntryBase
    {
        public string Ship { get; set; } // Note: ship type
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long ShipID { get; set; }
        public string UserShipName { get; set; }
        public string UserShipId { get; set; } // Note: ShipIdent elsewhere // TODO: check capitalisation
    }
}
