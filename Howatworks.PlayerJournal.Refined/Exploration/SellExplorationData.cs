using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    public class SellExplorationData : JournalEntryBase
    {
        public List<string> Systems { get; set; }
        public List<string> Discovered { get; set; }
        public int BaseValue { get; set; }
        public int Bonus { get; set; }
    }
}
