﻿namespace Howatworks.SubEtha.Journal.Combat
{
    public class Interdicted : JournalEntryBase
    {
        public bool Submitted { get; set; }
        public string Interdictor { get; set; } // NOTE: Commander name
        public bool IsPlayer { get; set; }
        public int? CombatRank { get; set; } // NOTE: Only for player // TODO: enum
        public string Faction { get; set; } // Note: faction name
        public string Power { get; set; } // Note: power name
    }
}