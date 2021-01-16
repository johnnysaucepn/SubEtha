using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierDecommision : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public long ScrapRefund { get; set; }
        public long ScrapTime { get; set; } // TODO: check data type
    }
}
