using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class QuitACrew : JournalEntryBase
    {
        public string Captain { get; set; } // Note: name
    }
}
