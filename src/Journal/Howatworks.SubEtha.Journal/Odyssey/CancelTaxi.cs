using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class CancelTaxi : JournalEntryBase
    {
        public long Refund { get; set; }
    }
}