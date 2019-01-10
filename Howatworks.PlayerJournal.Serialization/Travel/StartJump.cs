using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class StartJump : JournalEntryBase
    {
        public string JumpType { get; set; } // TODO: consider enum - Hyperspace, Supercruise
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; } // TODO: check data type
        public string StarClass { get; set; } // TODO: check data type
    }
}
