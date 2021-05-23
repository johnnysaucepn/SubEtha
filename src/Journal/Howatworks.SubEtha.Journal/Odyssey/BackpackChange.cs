using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class BackpackChange : JournalEntryBase
    {
        public class BackpackChangeItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            [JournalName("OwnerID")]
            public long OwnerId { get; set; } // TODO: defaults to 0 if no owner?
            [JournalName("MissonID")]
            public long? MissionId { get; set; }
            public int Count { get; set; }
            public string Type { get; set; } // TODO: enum?
        }

        public List<BackpackChangeItem> Added { get; set; }
        public List<BackpackChangeItem> Removed { get; set; }
    }
}