using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class EngineerContribution : JournalEntryBase
    {
        public string Engineer { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long EngineerID { get; set; } // TODO: check datatype
        public string Type { get; set; } // TODO: enum? (Commodity, materials, Credits, Bond, Bounty)
        public string Commodity { get; set; }
        public string Material { get; set; }
        public string Faction { get; set; }
        public int Quantity { get; set; }
        public int TotalQuantity { get; set; }
    }
}
