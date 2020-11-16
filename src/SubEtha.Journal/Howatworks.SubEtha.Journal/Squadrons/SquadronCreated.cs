using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Squadrons
{
    [ExcludeFromCodeCoverage]
    public class SquadronCreated : JournalEntryBase
    {
        public string SquadronName { get; set; }
    }
}
