namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class CrewMemberRoleChange : JournalEntryBase
    {
        public string Crew { get; set; }
        public string Role { get; set; } // TODO: enum?
    }
}
