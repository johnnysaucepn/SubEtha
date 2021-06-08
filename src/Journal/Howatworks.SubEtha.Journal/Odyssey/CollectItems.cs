using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class CollectItems : JournalEntryBase
    {
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public string Type { get; set; } // TODO: enum? Might be Encoded, Raw, Manufactured, Item, Component, Data, Consumable
        [JournalName("OwnerID")]
        public long OwnerId { get; set; } // TODO: defaults to 0 if no owner?
        public int Count { get; set; }
        public bool Stolen { get; set; }
    }
}