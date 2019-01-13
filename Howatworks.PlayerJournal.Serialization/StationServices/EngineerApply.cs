using System;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    [Obsolete] // As per v3.0
    public class EngineerApply : JournalEntryBase
    {
        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        // No example in document - name of override effect
        public string Override { get; set; }
    }
}
