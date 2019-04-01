using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Parser
{
    public interface IHandler
    {
        bool Invoke(IJournalEntry journal, BatchMode mode);
    }
}