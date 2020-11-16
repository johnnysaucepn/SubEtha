using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class KickCrewMember : JournalEntryBase
    {
        public string Crew { get; set; } // Note: name
        public bool OnCrime { get; set; }
    }
}
