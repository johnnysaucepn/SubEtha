﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class CrewMemberQuits : JournalEntryBase
    {
        public string Crew { get; set; } // Note: name
    }
}