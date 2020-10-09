using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal
{
    public interface INewJournalLineSource
    {
        IEnumerable<NewJournalLine> GetJournalLines();
    }
}
