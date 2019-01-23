namespace Howatworks.PlayerJournal.Serialization.Squadrons
{
    public class SquadronPromotion : JournalEntryBase
    {
        public string SquadronName { get; set; }
        public int OldRank { get; set; } // TODO: enum?
        public int NewRank { get; set; } // TODO: enum?
    }
}
