using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class BookTaxi : JournalEntryBase
    {
        public long Cost { get; set; }
        public string DestinationSystem { get; set; } // NOTE: starsystem name
        public string DestinationLocation { get; set; } // NOTE: starsystem name
    }
}