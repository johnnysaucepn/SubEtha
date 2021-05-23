using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class StoredShips : JournalEntryBase
    {
        public class ShipItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long ShipID { get; set; }
            public string ShipType { get; set; }
            public string ShipType_Localised { get; set; }
            public string Name { get; set; }
            public long Value { get; set; }
            public bool Hot { get; set; }
        }

        public class ShipRemoteItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long ShipID { get; set; }
            public string ShipType { get; set; }
            public string ShipType_Localised { get; set; }
            public string Name { get; set; }
            public long Value { get; set; }
            public bool Hot { get; set; }
            public bool? InTransit { get; set; }
            public string StarSystem { get; set; }
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long? ShipMarketID { get; set; }
            public long? TransferPrice { get; set; }
            public int TransferTime { get; set; } // WARNING: docs define TransferType, not found in sample or live, presume typo?
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public List<ShipItem> ShipsHere { get; set; }
        public List<ShipRemoteItem> ShipsRemote { get; set; }
    }
}
