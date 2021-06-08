using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class CancelDropship : JournalEntryBase
    {
        // NOTE: docs assume this is essentially same as CancelTaxi
        public long Refund { get; set; }
    }
}