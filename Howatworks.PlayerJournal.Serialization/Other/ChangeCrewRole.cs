using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class ChangeCrewRole : JournalEntryBase
    {
        public string Role { get; set; } // TODO: enum? Idle, FireCon, FighterCon
    }
}
