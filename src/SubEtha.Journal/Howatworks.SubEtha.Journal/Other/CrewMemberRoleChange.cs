using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class CrewMemberRoleChange : JournalEntryBase
    {
        public string Crew { get; set; }
        public string Role { get; set; } // TODO: enum?
    }
}
