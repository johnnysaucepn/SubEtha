using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierCancelDecommision : JournalEntryBase
    {
        public long CarrierID { get; set; }
    }
}
