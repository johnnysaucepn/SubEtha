﻿using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class CommitCrime : JournalEntryBase
    {
        public string CrimeType { get; set; } // TODO: enum?
        public string Faction { get; set; }

        #region Optional
        public string Victim { get; set; }
        public long? Fine { get; set; }
        public long? Bounty { get; set; }
        #endregion
    }
}
