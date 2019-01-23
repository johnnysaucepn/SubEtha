namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class ChangeCrewRole : JournalEntryBase
    {
        public string Role { get; set; } // TODO: enum? Idle, FireCon, FighterCon
    }
}
