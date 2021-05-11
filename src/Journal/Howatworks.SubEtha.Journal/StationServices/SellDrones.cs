using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class SellDrones : JournalEntryBase
    {
        public string Type { get; set; } // TODO: localised?
        public int Count { get; set; }
        public long SellPrice { get; set; }
        public long TotalSale { get; set; }
    }
}
