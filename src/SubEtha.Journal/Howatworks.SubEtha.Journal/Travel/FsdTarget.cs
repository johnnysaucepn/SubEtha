﻿namespace Howatworks.SubEtha.Journal.Travel
{
    [JournalName("FSDTarget")]
    public class FsdTarget : JournalEntryBase
    {
        public long SystemAddress { get; set; }
        public string Name { get; set; } // WARNING: docs say StarSystem
        public int RemainingJumpsInRoute { get; set; }
    }
}