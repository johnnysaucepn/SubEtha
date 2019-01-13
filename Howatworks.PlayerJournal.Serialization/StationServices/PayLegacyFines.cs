using System;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    [Obsolete]
    public class PayLegacyFines : JournalEntryBase
    {
        public int Amount { get; set; }
        public decimal? BrokerPercentage { get; set; }
    }
}
