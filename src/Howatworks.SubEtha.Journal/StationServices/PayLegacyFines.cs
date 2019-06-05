using System;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [Obsolete]
    public class PayLegacyFines : JournalEntryBase
    {
        public int Amount { get; set; }
        public decimal? BrokerPercentage { get; set; }
    }
}
