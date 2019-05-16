using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    public class DockingDenied : JournalEntryBase
    {
        public string StationName { get; set; }
        public string StationType { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        // TODO: consider enum - NoSpace, TooLarge, Hostile, Offences, Distance, ActiveFighter, NoReason
        public string Reason { get; set; }
    }
}
