using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Powerplay : JournalEntryBase
    {
        public string Power { get; set; } // Note: name
        public int Rank { get; set; }
        public int Merits { get; set; }
        public int Votes { get; set; }
        public long TimePledged { get; set; } // Note: time in seconds

    }
}
