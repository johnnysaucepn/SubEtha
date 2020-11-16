using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class BuyAmmo : JournalEntryBase
    {
        public long Cost { get; set; }
    }
}
