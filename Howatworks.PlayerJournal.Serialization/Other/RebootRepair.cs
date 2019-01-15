using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class RebootRepair : JournalEntryBase
    {
        public List<string> Modules { get; set; }
    }
}
