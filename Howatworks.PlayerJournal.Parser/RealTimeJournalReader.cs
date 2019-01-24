using System;
using System.Collections.Generic;
using System.IO;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Newtonsoft.Json.Linq;

namespace Howatworks.PlayerJournal.Parser
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

                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // TODO: beef up error handling here, what if line is not a parseable event?
                    var json = JObject.Parse(line);
                    var timestamp = json.Value<DateTime>("timestamp");

                    if (timestamp <= since) continue;

                    Log.Debug(line);
                    var eventType = json.Value<string>("event");

                    var journalEntry = _parser.Parse(eventType, line);

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
}
