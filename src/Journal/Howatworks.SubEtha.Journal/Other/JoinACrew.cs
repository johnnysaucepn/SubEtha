using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class JoinACrew : JournalEntryBase
    {
        public string Captain { get; set; } // Note: name
    }
}
