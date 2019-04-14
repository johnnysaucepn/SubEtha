using System;

namespace Howatworks.SubEtha.Monitor
{
    public interface IJournalMonitorState
    {
        DateTimeOffset? LastRead { get; set; }
        DateTimeOffset? LastChecked { get; set; }
    }
}