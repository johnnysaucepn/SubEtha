namespace Howatworks.PlayerJournal.Parser
{
    public interface IJournalParser
    {
        JournalEntryBase Parse(string gameVersion, string eventType, string line);
    }
}