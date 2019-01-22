namespace Howatworks.PlayerJournal.Parser
{
    public interface IJournalReaderFactory
    {
        IncrementalJournalReader CreateIncrementalJournalReader(string filePath);
        RealTimeJournalReader CreateRealTimeJournalReader(string filePath);
    }
}