namespace Howatworks.SubEtha.Parser
{
    public class JournalReaderFactory : IJournalReaderFactory
    {
        private readonly IJournalParser _parser;

        public JournalReaderFactory(IJournalParser parser)
        {
            _parser = parser;
        }

        public IncrementalJournalReader CreateIncrementalJournalReader(string filePath)
        {
            return new IncrementalJournalReader(filePath, _parser);
        }

        public RealTimeJournalReader CreateRealTimeJournalReader(string filePath)
        {
            return new RealTimeJournalReader(filePath, _parser);
        }
    }
}