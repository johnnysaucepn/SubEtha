namespace Howatworks.SubEtha.Parser
{
    public interface IJournalReaderFactory
    {
        IncrementalJournalReader CreateIncrementalJournalReader(string filePath);
        RealTimeJournalReader CreateRealTimeJournalReader(string filePath);
    }
}