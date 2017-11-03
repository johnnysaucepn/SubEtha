namespace Howatworks.PlayerJournal.Combat
{
    public class Bounty : JournalEntryBase
    {
        public class FactionReward
        {
            public string Faction { get; set; }
            public int Reward { get; set; }
        }

        public FactionReward[] Rewards { get; set; }
        public string Target { get; set; } // TODO: UNDOCUMENTED - ship type
        public string VictimFaction { get; set; }
        public string TotalReward { get; set; }
        public int SharedWithOthers { get; set; }

    }
}
