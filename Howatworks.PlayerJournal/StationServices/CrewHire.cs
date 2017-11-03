namespace Howatworks.PlayerJournal.StationServices
{
    public class CrewHire : JournalEntryBase
    {
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Cost { get; set; }
        // TODO: is rank really an integer?
        public int CombatRank { get; set; }
    }
}
