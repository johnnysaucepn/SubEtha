namespace Howatworks.PlayerJournal.Serialization.Combat
{
    public class Interdicted : JournalEntryBase
    {
        public bool Submitted { get; set; }
        public string Interdictor { get; set; } // NOTE: Commander name
        public bool IsPlayer { get; set; }
        public int CombatRank { get; set; } // NOTE: Only for player
        public string Faction { get; set; }
        public string Power { get; set; }
    }
}
