using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class RepairAll : JournalEntryBase
    {
        public long Cost { get; set; }
    }
}
