﻿using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class Undocked : JournalEntryBase
    {
        public string StationName { get; set; }
        public string StationType { get; set; } // WARNING: not in docs // TODO: enum? Coriolis, etc.
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
    }
}
