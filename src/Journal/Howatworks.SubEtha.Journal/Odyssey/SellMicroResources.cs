using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class SellMicroResources
    {
        public class MicroResourcesItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public string Category { get; set; } // TODO: enum? Encoded, Raw, Manufactured, Item, Component, Data, Consumable
            public int Count { get; set; }
        }
        public long Price { get; set; }
        [JournalName("MarketID")]
        public long MarketId { get; set; }
    }
}