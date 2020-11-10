using System.Collections.Generic;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Monitor
{
    public interface IJournalLineSource
    {
        IEnumerable<JournalLine> GetJournalLines();
    }
}
