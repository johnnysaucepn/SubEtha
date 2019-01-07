using System;
using System.Collections.Generic;
using System.IO;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalStatusReader : IJournalReader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalStatusReader));

        private readonly IJournalParser _parser;
        public DateTimeOffset? LastEntryTimeStamp { get; private set; }
        public string FilePath { get; }

        public JournalStatusReader(string filePath, IJournalParser parser)
        {
            FilePath = filePath;
            _parser = parser;
        }

        private static StreamReader GetStreamReader(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new StreamReader(fileStream);
        }

 
        public bool FileExists => File.Exists(FilePath);

        public IEnumerable<IJournalEntry> ReadAll(DateTimeOffset? since)
        {
            // Always get a new stream reader
            var streamReader = GetStreamReader(FilePath);
            
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                Log.Debug(line);
                if (string.IsNullOrWhiteSpace(line)) continue;

                // TODO: beef up error handling here, what if line is not a parseable event?
                var json = JObject.Parse(line);
                var timestamp = json.Value<DateTime>("timestamp");

                if (timestamp <= since.GetValueOrDefault(DateTime.MinValue)) continue;

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
