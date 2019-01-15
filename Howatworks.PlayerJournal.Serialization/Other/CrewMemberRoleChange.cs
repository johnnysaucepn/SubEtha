using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class CrewMemberRoleChange : JournalEntryBase
    {
        public string Crew { get; set; }
        public string Role { get; set; } // TODO: enum?
    }
}
