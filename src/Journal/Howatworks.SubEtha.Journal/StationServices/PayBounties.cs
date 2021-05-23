using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class PayBounties : JournalEntryBase
    {
        public long Amount { get; set; }
        public decimal? BrokerPercentage { get; set; }
        public bool AllFines { get; set; }
        public string Faction { get; set; }
        public string Faction_Localised { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long ShipID { get; set; }
    }
}
