using System;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    [Obsolete]
    public class PayLegacyFines : JournalEntryBase
    {
        public int Amount { get; set; }
        public decimal? BrokerPercentage { get; set; }
    }
}
