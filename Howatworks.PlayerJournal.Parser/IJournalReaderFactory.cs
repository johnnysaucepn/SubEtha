namespace Howatworks.PlayerJournal.Parser
{
    public interface IJournalReaderFactory
    {
        IJournalReader Create(string filePath);
    }
}