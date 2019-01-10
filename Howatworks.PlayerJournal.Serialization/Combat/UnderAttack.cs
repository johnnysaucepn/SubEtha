using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Combat
{
    // TODO: no sample
    public class UnderAttack : JournalEntryBase
    {
        public string Target { get; set; } // TODO: check type, expect enum Figher/Mothership/You
    }
}
