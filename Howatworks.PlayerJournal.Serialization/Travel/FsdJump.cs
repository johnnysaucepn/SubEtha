namespace Howatworks.PlayerJournal.Serialization.Travel
{
    [JournalName("FSDJump")]
    public class FsdJump : JournalEntryBase
    {
        public class FactionItem
        {
            public string Name { get; set; }
            public string FactionState { get; set; } // TODO: consider enum?
            public string Government { get; set; } // TODO: consider enum?
            public decimal Influence { get; set; }
            public decimal MyReputation { get; set; }
            public FactionItemStateItem[] PendingStates { get; set; }
            public FactionItemStateItem[] RecoveringStates { get; set; } // TODO: check spelling?
            public FactionItemStateItem[] ActiveStates { get; set; } // TODO: No trend value, check this is okay
            public bool SquadronFaction { get; set; }
            public bool HappiestSystem { get; set; }
            public bool HomeSystem { get; set; }
        }

        public class FactionItemStateItem
        {
            public string State { get; set; } // TODO: check name
            public string Trend { get; set; } // TODO: check data type?
        }

        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public decimal[] StarPos { get; set; }
        // TODO: Report that Body is never populated
        public string Body { get; set; } // Note: name
        public decimal JumpDist { get; set; }
        public decimal FuelUsed { get; set; }
        public decimal FuelLevel { get; set; }
        public bool BoostUsed { get; set; }
        public string SystemFaction { get; set; }
        public string FactionState { get; set; }
        public string SystemAllegiance { get; set; }
        public string SystemEconomy { get; set; }
        public string SystemEconomy_Localised { get; set; }
        public string SystemSecondEconomy { get; set; }
        public string SystemSecondEconomy_Localised { get; set; } // TODO: assumed
        public string SystemGovernment { get; set; }
        public string SystemGovernment_Localised { get; set; }
        public string SystemSecurity { get; set; }
        public string SystemSecurity_Localised { get; set; }
        public long Population { get; set; }
        public bool Wanted { get; set; } // TODO: check data type
        public FactionItem[] Factions { get; set; }
        public string[] Powers { get; set; }
        // TODO: consider enum -  ("InPrepareRadius", "Prepared", "Exploited", "Contested", "Controlled", "Turmoil", "HomeSystem")
        public string PowerplayState { get; set; }

    }
}
