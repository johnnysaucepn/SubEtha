using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Parser
{
    public interface IHandler
    {
        bool Invoke(IJournalEntry journal, BatchMode mode);
    }
}