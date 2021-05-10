using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class NpcCrewPaidWage : JournalEntryBase
    {
        public string NpcCrewId { get; set; } // TODO: check data type
        public string NpcCrewName { get; set; }
        public int RankCombat { get; set; } // TODO: enum?
    }
}
