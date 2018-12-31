using Howatworks.PlayerJournal.Serialization;

namespace Thumb.Plugin
{
    public interface IJournalProcessor
    {
        bool Apply(IJournalEntry journalEntry);
        void Flush();
    }
}