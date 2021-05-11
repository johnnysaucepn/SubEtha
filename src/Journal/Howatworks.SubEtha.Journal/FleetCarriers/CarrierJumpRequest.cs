using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierJumpRequest : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public string SystemName { get; set; }
        public string Body { get; set; } // NOTE: name
        public long SystemAddress { get; set; }
        public int BodyID { get; set; }
    }
}
