using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class DatalinkScan : JournalEntryBase
    {
        public string Message { get; set; }
    }
}
