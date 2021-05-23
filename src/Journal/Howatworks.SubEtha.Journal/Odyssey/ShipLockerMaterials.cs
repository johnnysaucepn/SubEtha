using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class ShipLockerMaterials : JournalEntryBase
    {
        // NOTE: currently identical to BackpackItem
        public class MaterialItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            [JournalName("OwnerID")]
            public long OwnerId { get; set; }
            [JournalName("MissonID")]
            public long MissionId { get; set; }
            public int Count { get; set; }
        }

        public List<MaterialItem> Items { get; set; }
        public List<MaterialItem> Components { get; set; }
        public List<MaterialItem> Consumables { get; set; }
        public List<MaterialItem> Data { get; set; }
    }
}