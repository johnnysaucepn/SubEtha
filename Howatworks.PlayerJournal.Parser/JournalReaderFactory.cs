namespace Howatworks.PlayerJournal.Parser
{
    public class JournalReaderFactory
    {
        private readonly JournalParser _parser;

        public JournalReaderFactory(JournalParser parser)
        {
            _parser = parser;
        }
        
        public JournalReader Create(string filePath)
        {
            return new JournalReader(filePath, _parser);
        }
    }
}