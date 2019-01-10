using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class DockingGranted : JournalEntryBase
    {
        public string StationName { get; set; }
        public string StationType { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; } // TODO: check data type
        public int LandingPad { get; set; }
    }
}
