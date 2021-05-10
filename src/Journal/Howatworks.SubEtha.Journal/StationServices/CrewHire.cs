using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class CrewHire : JournalEntryBase
    {
        public string Name { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long CrewID { get; set; } // TODO: check data type, not in sample
        public string Faction { get; set; }
        public long Cost { get; set; }
        public int CombatRank { get; set; } // TODO: enum?
    }
}
