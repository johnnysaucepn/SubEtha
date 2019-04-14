using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class RedeemVoucher : JournalEntryBase
    {
        public class FactionItem
        {
            public string Faction { get; set; }
            public int Amount { get; set; }
        }

        public string Type { get; set; } // TODO: enum: (CombatBond/Bounty/Trade/Settlement/Scannable)
        public int Amount { get; set; }
        public string Faction { get; set; }
        public decimal? BrokerPercentage { get; set; }
        public List<FactionItem> Factions { get; set; }
    }
}
