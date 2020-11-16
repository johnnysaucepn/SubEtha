using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class Friends : JournalEntryBase
    {
        public string Status { get; set; } // TODO: enum? Requested, Declined, Added, Lost, Offline, Online
        public string Name { get; set; }
    }
}
