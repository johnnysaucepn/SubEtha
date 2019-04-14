using System;
using System.Collections.Generic;
using System.IO;
using Howatworks.SubEtha.Journal;
using log4net;

namespace Howatworks.SubEtha.Parser
{
    public class RealTimeJournalReader : IJournalReader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RealTimeJournalReader));

        private readonly IJournalParser _parser;
        public DateTimeOffset? LastEntryTimeStamp { get; private set; }
        public string FilePath { get; }

        public RealTimeJournalReader(string filePath, IJournalParser parser)
        {
            FilePath = filePath;
            _parser = parser;
        }

        private StreamReader GetStreamReader()
        {
            var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new StreamReader(fileStream);
        }

        public bool FileExists => File.Exists(FilePath);

        public IEnumerable<IJournalEntry> ReadAll(DateTimeOffset since)
        {
            // Always get a new stream reader
            using (var streamReader = GetStreamReader())
            {
                var content = streamReader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(content)) yield break;

                // TODO: beef up error handling here, what if content is not a parseable event, or is multiple events?
                var (eventType, timestamp) = _parser.ParseCommonProperties(content);

                if (timestamp <= since) yield break;

                Log.Debug(content);

                var journalEntry = _parser.Parse(eventType, content);

                // TODO: remove this check once we're confident we should recognise all types
                if (journalEntry != null)
                {
                    LastEntryTimeStamp = journalEntry.Timestamp;
                    yield return journalEntry;
                }
            }
        }

    }
}
