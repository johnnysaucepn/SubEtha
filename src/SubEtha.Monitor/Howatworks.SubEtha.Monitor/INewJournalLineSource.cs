using System.Collections.Generic;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Monitor
{
    public interface INewJournalLineSource
    {
        IEnumerable<NewJournalLine> GetJournalLines();
    }
}
