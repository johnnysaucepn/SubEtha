using System.IO;
using Howatworks.SubEtha.Parser;

namespace Howatworks.SubEtha.Monitor
{
    public class JournalReaderFactory : IJournalReaderFactory
    {
        private readonly IJournalParser _parser;

        public JournalReaderFactory(IJournalParser parser)
        {
            _parser = parser;
        }

        public LogJournalReader CreateLogJournalReader(FileInfo file)
        {
            return new LogJournalReader(file, _parser);
        }

        public LiveJournalReader CreateLiveJournalReader(FileInfo file)
        {
            return new LiveJournalReader(file, _parser);
        }
    }
}