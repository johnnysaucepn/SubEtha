using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Squadrons
{
    [ExcludeFromCodeCoverage]
    public class AppliedToSquadron : JournalEntryBase
    {
        public string SquadronName { get; set; }
    }
}
