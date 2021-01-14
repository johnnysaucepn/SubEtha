using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierNameChanged : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public string Callsign { get; set; }
        public string Name { get; set; }
    }
}
