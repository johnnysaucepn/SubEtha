using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Combat
{
    public class Died : JournalEntryBase
    {
        public string KillerName { get; set; } // NOTE: Commander name
        public string KillerName_Localised { get; set; }
        public string KillerRank { get; set; } // NOTE: Rank currently string, not enum

        public class Killer
        {
            public string Name { get; set; } // NOTE: Commander name
            public string Ship { get; set; }
            public string Rank { get; set; } // NOTE: Rank currently string, not enum
        }

        public List<Killer> Killers { get; set; }

    }
}
