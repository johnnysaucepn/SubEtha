using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Processing
{
    public interface IJournalProcessor
    {
        bool Apply(IJournalEntry journalEntry);
        void Flush();
    }
}