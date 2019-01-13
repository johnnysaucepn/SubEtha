using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class PayBounties : JournalEntryBase
    {
        public int Amount { get; set; }
        public decimal? BrokerPercentage { get; set; }
        public bool AllFines { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
    }
}
