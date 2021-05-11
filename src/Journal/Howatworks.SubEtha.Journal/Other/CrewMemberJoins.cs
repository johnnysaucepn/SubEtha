using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class CrewMemberJoins : JournalEntryBase
    {
        public string Crew { get; set; } // Note: name
    }
}
