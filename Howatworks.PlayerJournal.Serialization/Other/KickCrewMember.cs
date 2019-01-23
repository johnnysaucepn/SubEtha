namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class KickCrewMember : JournalEntryBase
    {
        public string Crew { get; set; } // Note: name
        public bool OnCrime { get; set; }
    }
}
