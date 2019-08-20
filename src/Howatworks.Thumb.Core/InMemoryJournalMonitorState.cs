using System;
using Howatworks.SubEtha.Monitor;

namespace Howatworks.Thumb.Core
{
    public class InMemoryJournalMonitorState : IJournalMonitorState
    {
        public DateTimeOffset? LastEntrySeen { get; private set; }
        public DateTimeOffset? LastChecked { get; private set; }

        public void Update(DateTimeOffset lastChecked, DateTimeOffset lastEntrySeen)
        {
            LastChecked = lastChecked;
            LastEntrySeen = lastEntrySeen;
        }
    }
}