using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Other
{
    public class Synthesis : JournalEntryBase
    {
        // TODO: localised?
        public string Name { get; set; }
        // TODO: enough usages to make a distinct type?
        public Dictionary<string, int> Materials { get; set; }
    }
}
