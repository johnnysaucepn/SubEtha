using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Combat
{
    // Note: no sample
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class ShipTargetted : JournalEntryBase
    {
        public bool TargetLocked { get; set; }

        // If target locked:
        public string Ship { get; set; } // Note: ship name
        public int? ScanStage { get; set; } // TODO: enumerate these?

        // If ScanStage >=1
        public string PilotName { get; set; } // Note: commander name
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
