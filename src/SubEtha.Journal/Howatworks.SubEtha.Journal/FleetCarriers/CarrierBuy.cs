using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierBuy : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public long BoughtAtMarket { get; set; } // NOTE: market id
        public string Location { get; set; }
        public long SystemAddress { get; set; }
        public long Price { get; set; }
        public string Variant { get; set; } // TODO: enum
        public string Callsign { get; set; }
    }
}
