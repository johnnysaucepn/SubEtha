using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Squadrons
{
    [ExcludeFromCodeCoverage]
    public class KickedFromSquadron : JournalEntryBase
    {
        public string SquadronName { get; set; }
    }
}
