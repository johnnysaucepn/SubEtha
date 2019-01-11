using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    public class MultiSellExplorationData : JournalEntryBase
    {
        public class DiscoveredItem
        {
            public string SystemName { get; set; }
            public int NumBodies { get; set; }
        }

        public List<DiscoveredItem> Discovered { get; set; }
        public int BaseValue { get; set; }
        public int Bonus { get; set; }
        public int TotalEarnings { get; set; }
    }
}
