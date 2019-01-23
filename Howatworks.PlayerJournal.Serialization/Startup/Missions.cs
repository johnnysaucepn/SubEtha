using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        public List<MissionItem> Active { get; set; }
        public List<MissionItem> Failed { get; set; }
        public List<MissionItem> Complete { get; set; }
    }
}
