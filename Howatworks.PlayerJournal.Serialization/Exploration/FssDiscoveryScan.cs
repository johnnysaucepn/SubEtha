using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    // Note: no sample
    [JournalName("FSSDiscoveryScan")]
    public class FssDiscoveryScan : JournalEntryBase
    {
        public decimal Progress { get; set; }
        public int BodyCount { get; set; }
        public int NonBodyCount { get; set; }
    }
}
