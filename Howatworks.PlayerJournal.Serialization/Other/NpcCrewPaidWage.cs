using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class NpcCrewPaidWage : JournalEntryBase
    {
        public string NpcCrewId { get; set; } // TODO: check data type
        public string NpcCrewName { get; set; }
        public int RankCombat { get; set; } // TODO: enum?
    }
}
