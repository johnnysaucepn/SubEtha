using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class UseConsumable : JournalEntryBase
    {
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public string Type { get; set; } // WARN: Not Category public string Category { get; set; } // TODO: enum? Encoded, Raw, Manufactured, Item, Component, Data, Consumable
    }
}