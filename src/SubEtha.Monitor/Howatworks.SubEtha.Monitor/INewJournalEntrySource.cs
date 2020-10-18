using System.Collections.Generic;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Monitor
{
    public interface INewJournalEntrySource
    {
        IEnumerable<NewJournalEntry> GetJournalEntries();
    }
}