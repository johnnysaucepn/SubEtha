﻿using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Combat
{
    [ExcludeFromCodeCoverage]
    // TODO: no sample
    public class UnderAttack : JournalEntryBase
    {
        public string Target { get; set; } // TODO: check type, expect enum Figher/Mothership/You
    }
}
