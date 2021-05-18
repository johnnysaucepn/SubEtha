using System.Collections.Generic;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Parser;

namespace Howatworks.SubEtha.Monitor
{
    public interface IJournalLineSource
    {
        IEnumerable<JournalResult<JournalLine>> GetJournalLines();
    }
}
