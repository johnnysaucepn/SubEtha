using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class TransferMicroResources : JournalEntryBase
    {
        public class TransferItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public string Category { get; set; } // TODO: enum? Encoded, Raw, Manufactured, Item, Component, Data, Consumable
            public int Count { get; set; }
            public string Direction { get; set; } // TODO: enum? ToBackpack, ToShipLocker, any others?
        }

        public List<TransferItem> Transfers { get; set; }
    }
}