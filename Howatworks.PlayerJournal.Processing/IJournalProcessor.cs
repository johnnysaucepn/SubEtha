namespace Howatworks.PlayerJournal.Processing
{
    public interface IJournalProcessor
    {
        bool Apply(JournalEntryBase entry);
        void Flush();
    }
}