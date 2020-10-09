using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal
{
    public interface INewJournalEntrySource
    {
        IEnumerable<NewJournalEntry> GetJournalEntries();
    }
}