using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Parser
{
    public interface IJournalProcessor
    {
        bool Apply(IJournalEntry journalEntry);
        void Flush();
    }
}