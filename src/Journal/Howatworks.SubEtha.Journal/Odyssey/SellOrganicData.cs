using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class SellOrganicData : JournalEntryBase
    {
        public class BioDataItem
        {
            public string Genus { get; set; } // TODO: check if localised
            public string Species { get; set; } // TODO: check type, check if localised
            public long Value { get; set; }
            public long Bonus { get; set; }
        }

        public long MarketId { get; set; }
        public List<BioDataItem> BioData { get; set; }
    }
}