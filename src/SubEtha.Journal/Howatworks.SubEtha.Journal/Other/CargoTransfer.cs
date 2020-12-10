using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.Other
{
    public class CargoTransfer : JournalEntryBase
    {
        public class TransferItem
        {
            public string Type { get; set; }
            public int Count { get; set; }
            public string Direction { get; set; } // TODO: enum?
        }

        public List<TransferItem> Transfers { get; set; }
    }
}
