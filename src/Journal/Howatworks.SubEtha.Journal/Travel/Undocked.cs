using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    [ExcludeFromCodeCoverage]
    public class Undocked : JournalEntryBase
    {
        public string StationName { get; set; }
        public string StationType { get; set; } // WARNING: not in docs // TODO: enum? Coriolis, etc.
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
    }
}
