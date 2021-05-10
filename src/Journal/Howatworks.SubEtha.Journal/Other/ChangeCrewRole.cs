using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class ChangeCrewRole : JournalEntryBase
    {
        public string Role { get; set; } // TODO: enum? Idle, FireCon, FighterCon
    }
}
