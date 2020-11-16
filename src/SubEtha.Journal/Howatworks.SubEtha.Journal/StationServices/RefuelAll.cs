using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class RefuelAll : JournalEntryBase
    {
        public long Cost { get; set; }
        public decimal Amount { get; set; }
    }
}
