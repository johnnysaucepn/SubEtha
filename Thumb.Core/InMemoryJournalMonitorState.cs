using System;
using Howatworks.PlayerJournal.Monitor;

namespace Thumb.Core
{
    public class InMemoryJournalMonitorState : IJournalMonitorState
    {
        public DateTime? LastRead { get; set; }
    }
}