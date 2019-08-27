using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.Exploration
{
    public class MultiSellExplorationData : JournalEntryBase
    {
        public class DiscoveredItem
        {
            public string SystemName { get; set; }
            public int NumBodies { get; set; }
        }

        public List<DiscoveredItem> Discovered { get; set; }
        public long BaseValue { get; set; }
        public long Bonus { get; set; }
        public long TotalEarnings { get; set; }
    }
}
