using System.IO;

namespace Howatworks.SubEtha.Parser
{
    public class NewJournalReaderFactory : INewJournalReaderFactory
    {
        private readonly IJournalParser _parser;

        public NewJournalReaderFactory(IJournalParser parser)
        {
            _parser = parser;
        }

        public NewLogJournalReader CreateLogJournalReader(FileInfo file)
        {
            return new NewLogJournalReader(file, _parser);
        }

        public NewLiveJournalReader CreateLiveJournalReader(FileInfo file)
        {
            return new NewLiveJournalReader(file, _parser);
        }
    }
}