﻿using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Combat
{
    // Note: no sample
    [ExcludeFromCodeCoverage]
    [JournalName("PVPKill")]
    public class PvpKill : JournalEntryBase
    {
        public string Victim { get; set; } // NOTE: Commander name
        public int CombatRank { get; set; } // TODO: enum?
    }
}
