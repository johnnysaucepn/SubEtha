using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Missions : JournalEntryBase
    {
        public class MissionItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long MissionID { get; set; }
            public string Name { get; set; }
            public bool PassengerMission { get; set; }
            public int Expires { get; set; } // NOTE: in seconds
        }

        public MissionItem[] Active { get; set; }
        public MissionItem[] Failed { get; set; }
        public MissionItem[] Complete { get; set; }
    }
}
