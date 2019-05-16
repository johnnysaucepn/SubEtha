using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    // TODO: no sample
    public class DockingCancelled : JournalEntryBase
    {
        public string StationName { get; set; }
        public string StationType { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
    }
}
