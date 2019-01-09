using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Materials : JournalEntryBase
    {
        public class MaterialsItem
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }

        public MaterialsItem[] Raw { get; set; }
        public MaterialsItem[] Manufactured { get; set; }
        public MaterialsItem[] Encoded { get; set; }
    }
}
