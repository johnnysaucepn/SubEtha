using System;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Thumb.Core
{
    public class InMemoryJournalMonitorState : IJournalMonitorState
    {
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Setter required for deserialization")]
        public DateTimeOffset? LastEntrySeen { get; set; }
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Setter required for deserialization")]
        public DateTimeOffset? LastChecked { get; set; }

        public void Update(DateTimeOffset lastChecked, DateTimeOffset lastEntrySeen)
        {
            LastChecked = lastChecked;
            LastEntrySeen = lastEntrySeen;
        }
    }
}