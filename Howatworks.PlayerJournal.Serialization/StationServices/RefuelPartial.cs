﻿namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class RefuelPartial : JournalEntryBase
    {
        public long Cost { get; set; }
        public decimal Amount { get; set; }
    }
}
