using System;
using System.IO;

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
            if (filePath != null && Path.GetFileName(filePath).Equals("status.json", StringComparison.InvariantCultureIgnoreCase))
            {
                return new JournalStatusReader(filePath, _parser);
            }
            return new JournalLogReader(filePath, _parser);
        }
    }
}