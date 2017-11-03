using System.Collections.Generic;

namespace Howatworks.PlayerJournal.StationServices
{
    public class EngineerCraft : JournalEntryBase
    {
        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        public Dictionary<string, int> Ingredients { get; set; }
    }
}
