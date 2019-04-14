namespace Howatworks.SubEtha.Journal.Squadrons
{
    public class SquadronDemotion : JournalEntryBase
    {
        public string SquadronName { get; set; }
        public int OldRank { get; set; } // TODO: enum?
        public int NewRank { get; set; } // TODO: enum?
    }
}
