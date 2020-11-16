using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class RefuelPartial : JournalEntryBase
    {
        public long Cost { get; set; }
        public decimal Amount { get; set; }
    }
}
