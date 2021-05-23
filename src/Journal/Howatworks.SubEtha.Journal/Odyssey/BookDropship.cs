using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class BookDropship : JournalEntryBase
    {
        // NOTE: docs assume this is essentially same as BookTaxi
        public long Cost { get; set; }
        public string DestinationSystem { get; set; } // NOTE: starsystem name
        public string DestinationLocation { get; set; } // NOTE: starsystem name
    }
}