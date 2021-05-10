using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class PayFines : JournalEntryBase
    {
        public int Amount { get; set; }
        public decimal? BrokerPercentage { get; set; }
        public bool AllFines { get; set; }
        public string Faction { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
    }
}
