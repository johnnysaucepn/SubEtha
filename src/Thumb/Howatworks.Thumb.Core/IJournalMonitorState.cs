using System;

namespace Howatworks.Thumb.Core
{
    public interface IJournalMonitorState
    {
        DateTimeOffset? LastEntrySeen { get; }
        DateTimeOffset? LastChecked { get; }

        /// <summary>
        /// Update the internal representation and save
        /// </summary>
        void Update(DateTimeOffset lastChecked, DateTimeOffset lastEntrySeen);
    }
}