namespace Howatworks.PlayerJournal.Processing
{
    public interface IHandler
    {
        bool Invoke(JournalEntryBase journal);
    }
}