namespace Howatworks.PlayerJournal.Serialization.Combat
{
    // Note: no sample
    [JournalName("PVPKill")]
    public class PvpKill : JournalEntryBase
    {
        public string Victim { get; set; } // NOTE: Commander name
        public int CombatRank { get; set; }
    }
}
