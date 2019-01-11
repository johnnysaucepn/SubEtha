using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    // Note: no sample
    [JournalName("FSSAllBodiesFound")]
    public class FssAllBodiesFound : JournalEntryBase
    {
        public string SystemName { get; set; }
        public long SystemAddress { get; set; }
        public int Count { get; set; }
    }
}
