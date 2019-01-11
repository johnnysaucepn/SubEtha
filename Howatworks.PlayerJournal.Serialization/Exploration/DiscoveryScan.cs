using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    // Note: no sample
    public class DiscoveryScan : JournalEntryBase
    {
        public long SystemAddress { get; set; }
        public int Bodies { get; set; }
    }
}
