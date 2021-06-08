using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    [JournalName("BackPack")]
    public class Backpack : JournalEntryBase
    {
        public class BackpackItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            [JournalName("OwnerID")]
            public long OwnerId { get; set; }
            [JournalName("MissonID")]
            public long MissionId { get; set; }
            public int Count { get; set; }
        }

        public List<BackpackItem> Items { get; set; }
        public List<BackpackItem> Components { get; set; }
        public List<BackpackItem> Consumables { get; set; }
        public List<BackpackItem> Data { get; set; }
    }
}