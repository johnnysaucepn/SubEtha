﻿namespace Howatworks.SubEtha.Journal.Other
{
    public class AmfuRepairs : JournalEntryBase
    {
        public string Model { get; set; } // Note: name
        public bool FullyRepaired { get; set; }
        public decimal Health { get; set; } // Note: 0.0-1.0
    }
}