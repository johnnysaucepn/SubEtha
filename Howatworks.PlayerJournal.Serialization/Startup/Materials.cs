using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Materials : JournalEntryBase
    {
        public class MaterialItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public int Count { get; set; }
        }

        public List<MaterialItem> Raw { get; set; }
        public List<MaterialItem> Manufactured { get; set; }
        public List<MaterialItem> Encoded { get; set; }
    }
}
