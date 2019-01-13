using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class MissionRedirected : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MissionID { get; set; }
        public string Name { get; set; }
        public string NewDestinationStation { get; set; } // Note: name
        public string OldDestinationStation { get; set; } // Note: name
        public string NewDestinationSystem { get; set; } // Note: name
        public string OldDestinationSystem { get; set; } // Note: name

        // TODO: check if SystemAddress is logged for any of these

    }
}
