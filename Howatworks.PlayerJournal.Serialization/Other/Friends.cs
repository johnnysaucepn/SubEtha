using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class Friends : JournalEntryBase
    {
        public string Status { get; set; } // TODO: enum? Requested, Declined, Added, Lost, Offline, Online
        public string Name { get; set; }
    }
}
