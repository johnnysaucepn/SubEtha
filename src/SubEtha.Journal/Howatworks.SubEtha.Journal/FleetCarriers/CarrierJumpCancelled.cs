using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierJumpCancelled : JournalEntryBase
    {
        public long CarrierID { get; set; }
    }
}
