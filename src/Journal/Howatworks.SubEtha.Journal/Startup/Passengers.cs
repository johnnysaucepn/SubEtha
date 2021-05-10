using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Startup
{
    [ExcludeFromCodeCoverage]
    public class Passengers : JournalEntryBase
    {
        // NOTE: no sample
        public class ManifestItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long MissionID { get; set; } // TODO: check data type
            public string Type { get; set; } // NOTE: type of passenger // TODO: check data type
            public bool VIP { get; set; }
            public bool Wanted { get; set; }
            public int Count { get; set; }
        }

        public List<ManifestItem> Manifest { get; set; }
    }
}
