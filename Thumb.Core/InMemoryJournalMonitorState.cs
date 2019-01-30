using System;
using Howatworks.PlayerJournal.Monitor;

namespace Thumb.Core
{
    public class InMemoryJournalMonitorState : IJournalMonitorState
    {
        public DateTimeOffset? LastRead { get; set; }
        public DateTimeOffset? LastChecked { get; set; }
    }
}