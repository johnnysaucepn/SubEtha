using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    [ExcludeFromCodeCoverage]
    public class DockingRequested : JournalEntryBase
    {
        public string StationName { get; set; }
        public string StationType { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
    }
}
