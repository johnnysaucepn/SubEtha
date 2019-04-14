namespace Howatworks.SubEtha.Journal.Other
{
    [JournalName("LaunchSRV")]
    public class LaunchSrv : JournalEntryBase
    {
        public string Loadout { get; set; } // TODO: enum?
        public bool PlayerControlled { get; set; } // WARNING: undocumented
    }
}
