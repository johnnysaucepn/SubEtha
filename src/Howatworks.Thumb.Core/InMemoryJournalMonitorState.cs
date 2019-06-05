using System;
using Howatworks.SubEtha.Monitor;

namespace Howatworks.Thumb.Core
{
    public class InMemoryJournalMonitorState : IJournalMonitorState
    {
        public DateTimeOffset? LastRead { get; set; }
        public DateTimeOffset? LastChecked { get; set; }
    }
}