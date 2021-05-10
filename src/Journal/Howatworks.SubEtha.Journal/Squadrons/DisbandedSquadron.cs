using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Squadrons
{
    [ExcludeFromCodeCoverage]
    public class DisbandedSquadron : JournalEntryBase
    {
        public string SquadronName { get; set; }
    }
}
