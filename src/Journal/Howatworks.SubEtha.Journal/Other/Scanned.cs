using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class Scanned : JournalEntryBase
    {
        public string ScanType { get; set; } // TODO: enum - Cargo, Crime, Cabin, Data or Unknown
    }
}
