namespace Howatworks.SubEtha.Journal.Squadrons
{
    public class SquadronStartup : JournalEntryBase
    {
        public string SquadronName { get; set; }
        public int CurrentRank { get; set; } // TODO: enum?
    }
}
