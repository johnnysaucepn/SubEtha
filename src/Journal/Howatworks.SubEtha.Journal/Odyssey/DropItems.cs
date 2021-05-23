using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class DropItems : JournalEntryBase // TODO: check this
    {
        public string Name { get; set; }
        public string Name_Localised { get; set; } // TODO: check this
        public string Type { get; set; } // TODO: enum? Might be Encoded, Raw, Manufactured, Item, Component, Data, Consumable
        [JournalName("OwnerID")]
        public long OwnerId { get; set; } // TODO: defaults to 0 if no owner?
        [JournalName("MissonID")]
        public long? MissionId { get; set; }
        public int Count { get; set; }
    }
}