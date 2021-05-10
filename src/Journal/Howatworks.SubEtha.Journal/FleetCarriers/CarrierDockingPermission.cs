using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierDockingPermission : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public string DockingAccess { get; set; } // TODO: enum - all/none/friends/squadron/squadronfriends
        public bool AllowNotorious { get; set; }
    }
}
