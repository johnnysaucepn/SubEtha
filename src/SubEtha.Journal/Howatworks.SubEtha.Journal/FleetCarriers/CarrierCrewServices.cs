using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierCrewServices : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public string Operation { get; set; } // TODO: enum (activate/deactivate/pause/resume/replace)
        public string CrewRole { get; set; } // TODO: enum?
        public string CrewName { get; set; }
    }
}
