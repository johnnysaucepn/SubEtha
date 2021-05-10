using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Trade
{
    [ExcludeFromCodeCoverage]
    public class BuyTradeData : JournalEntryBase
    {
        public string System { get; set; } // Note: name
        public long Cost { get; set; }
    }
}
