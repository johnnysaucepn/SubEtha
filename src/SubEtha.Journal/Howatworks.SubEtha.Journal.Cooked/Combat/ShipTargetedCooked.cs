using Howatworks.SubEtha.Journal.Combat;

namespace Howatworks.SubEtha.Journal.Cooked.Combat
{
    // Note: no sample
    public class ShipTargetedCooked : JournalEntryBase
    {
        public JournalLogFileInfo Context { get; }

        public bool TargetLocked { get; }

        // If target locked:
        public Localised<string> Ship { get; } // Note: ship type
        public int ScanStage { get; } // TODO: enumerate these?

        // If ScanStage >=1
        public Localised<string> PilotName { get; } // Note: commander name
        public string PilotRank { get; } // Note: rank name, not enum

        // If ScanStage >=2
        public decimal? ShieldHealth { get;  } // TODO: check data type
        public decimal? HullHealth { get; } // TODO: check data type

        // If ScanStage >=2
        public string Faction { get; } // Note: faction name
        public string LegalStatus { get; } // TODO: check data type
        public decimal? Bounty { get; }
        public string SubSystem { get; } // TODO: check data type
        public decimal? SubSystemHealth { get; } // TODO: check data type
        public string Power { get; } // TODO: based on assumption that all Powers are NPC names

        public ShipTargetedCooked(JournalLogFileInfo context, ShipTargeted source)
        {
            Context = context;

            TargetLocked = source.TargetLocked;
            Ship = new Localised<string>(source.Ship, source.Ship_Localised);
            ScanStage = source.ScanStage ?? 0;

            PilotName = new Localised<string>(source.PilotName, source.PilotName_Localised);
            PilotRank = source.PilotRank;

            ShieldHealth = source.ShieldHealth;
            HullHealth = source.HullHealth;

            Faction = source.Faction;
            LegalStatus = source.LegalStatus;
            Bounty = source.Bounty;
            SubSystem = source.SubSystem;
            SubSystemHealth = source.SubSystemHealth;
            Power = source.Power;
        }
    }
}
