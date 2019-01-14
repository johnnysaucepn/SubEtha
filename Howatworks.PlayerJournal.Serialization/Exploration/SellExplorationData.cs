using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    public class SellExplorationData : JournalEntryBase
    {
        public List<string> Systems { get; set; }
        public List<string> Discovered { get; set; }
        public long BaseValue { get; set; }
        public long Bonus { get; set; }
        public long TotalEarnings { get; set; }
    }
}
