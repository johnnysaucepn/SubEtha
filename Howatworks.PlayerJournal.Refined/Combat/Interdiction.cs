namespace Howatworks.PlayerJournal.Serialization.Combat
{
    public class Interdiction : JournalEntryBase
    {
        public bool Success { get; set; }
        public string Interdicted { get; set; } // NOTE: Commander name
        public bool IsPlayer { get; set; }
        public int CombatRank { get; set; } // NOTE: Enum, not string
        public string Faction { get; set; }
        public string Power { get; set; }
    }
}
