using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Squadrons
{
    [ExcludeFromCodeCoverage]
    public class SquadronStartup : JournalEntryBase
    {
        public string SquadronName { get; set; }
        public int CurrentRank { get; set; } // TODO: enum?
    }
}
