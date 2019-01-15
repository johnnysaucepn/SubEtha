namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class LaunchFighter : JournalEntryBase
    {
        public string Loadout { get; set; } // TODO: enum?
        public bool PlayerControlled { get; set; }
    }
}
