namespace Howatworks.SubEtha.Journal.Combat
{
    // Note: no sample
    public class ShipTargeted : JournalEntryBase // WARNING: named ShipTargetted in docs
    {
        public bool TargetLocked { get; set; }

        // If target locked:
        public string Ship { get; set; } // Note: ship name
        public string Ship_Localised { get; set; }
        public int? ScanStage { get; set; } // TODO: enumerate these?

        // If ScanStage >=1
        public string PilotName { get; set; } // Note: commander name
        public string PilotName_Localised { get; set; }
        public string PilotRank { get; set; } // Note: rank name, not enum

        // If ScanStage >=2
        public decimal? ShieldHealth { get; set; } // TODO: check data type
        public decimal? HullHealth { get; set; } // TODO: check data type

        // If ScanStage >=2
        public string Faction { get; set; } // Note: faction name
        public string LegalStatus { get; set; } // TODO: check data type
        public decimal? Bounty { get; set; } // TODO: check data type
        public string SubSystem { get; set; } // TODO: check data type
        public decimal? SubSystemHealth { get; set; } // TODO: check data type

    }
}
