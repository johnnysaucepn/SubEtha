namespace Howatworks.PlayerJournal.Serialization.Combat
{
    [JournalName("PVPKill")]
    public class PvpKill : JournalEntryBase
    {
        public string Victim { get; set; } // NOTE: Commander name
        public int CombatRank { get; set; } // NOTE: Enum, not string
    }
}
