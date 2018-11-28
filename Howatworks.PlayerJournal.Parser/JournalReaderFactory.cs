namespace Howatworks.PlayerJournal.Parser
{
    public class JournalReaderFactory : IJournalReaderFactory
    {
        private readonly IJournalParser _parser;

        public JournalReaderFactory(IJournalParser parser)
        {
            _parser = parser;
        }
        
        public IJournalReader Create(string filePath)
        {
            return new JournalReader(filePath, _parser);
        }
    }
}