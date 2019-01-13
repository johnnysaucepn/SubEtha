using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class SellShipOnRebuy : JournalEntryBase
    {
        public string ShipType { get; set; }
        public string System { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int SellShipID { get; set; } // TODO: Check capitalisation
        public int ShipPrice { get; set; }
    }
}
