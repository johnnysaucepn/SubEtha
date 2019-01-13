using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class CrewHire : JournalEntryBase
    {
        public string Name { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long CrewID { get; set; } // TODO: check data type
        public string Faction { get; set; }
        public string Cost { get; set; }
        public int CombatRank { get; set; } // TODO: enum?
    }
}
