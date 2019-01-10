using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Serialization.Combat
{
    public class Died : JournalEntryBase
    {
        public class KillerItem
        {
            public string Name { get; set; } // NOTE: Commander name
            public string Ship { get; set; } // NOTE: ship type
            public string Rank { get; set; } // NOTE: Rank currently string, not enum
        }

        // The following properties are for single kills only
        public string KillerName { get; set; } // NOTE: Commander name
        public string KillerName_Localised { get; set; }
        public string KillerShip { get; set; } // NOTE: ship type
        public string KillerRank { get; set; } // NOTE: Rank currently string, not enum

        // The following properties are for wing kills only
        public List<KillerItem> Killers { get; set; }

    }
}
