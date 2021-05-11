using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Squadrons
{
    [ExcludeFromCodeCoverage]
    public class SquadronDemotion : JournalEntryBase
    {
        public string SquadronName { get; set; }
        public int OldRank { get; set; } // TODO: enum?
        public int NewRank { get; set; } // TODO: enum?
    }
}
