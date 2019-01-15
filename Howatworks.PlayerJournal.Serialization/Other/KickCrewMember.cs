using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class KickCrewMember : JournalEntryBase
    {
        public string Crew { get; set; } // Note: name
        public bool OnCrime { get; set; }
    }
}
