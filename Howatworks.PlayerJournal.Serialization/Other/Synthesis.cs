using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class Synthesis : JournalEntryBase
    {
        public class MaterialItem
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }

        // TODO: localised?
        public string Name { get; set; }
        // TODO: enough usages to make a distinct type?
        public List<MaterialItem> Materials { get; set; }
    }
}
