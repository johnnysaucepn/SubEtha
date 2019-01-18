using System;
using System.Collections.Generic;
using System.IO;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalLogReader : IJournalReader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalLogReader));

        private readonly IJournalParser _parser;
        private readonly Lazy<StreamReader> _streamReader;
        public string FilePath { get; }
        private JournalLogFileInfo FileInfo { get; }
        public DateTimeOffset? LastEntryTimeStamp { get; private set; }

        public JournalLogReader(string filePath, IJournalParser parser)
        {
            _parser = parser;
            FilePath = filePath;
            FileInfo = ReadFileInfo(filePath);
            _streamReader = new Lazy<StreamReader>(() => GetStreamReader(filePath));
        }

        private StreamReader GetStreamReader(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new StreamReader(fileStream);
        }

        public bool FileExists => File.Exists(FileInfo.Path);

        private JournalLogFileInfo ReadFileInfo(string filePath)
        {
            FileHeader fileHeader = null;
            var info = new JournalLogFileInfo(filePath);
            DateTime? lastEntry = null;

            try
            {
                var streamReader = GetStreamReader(filePath);

                // FileHeader *should* be the first line in the file, but at least try the first 5
                var tolerance = 5;

                while (!streamReader.EndOfStream && tolerance > 0 && fileHeader == null)
                {
                    var line = streamReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // TODO: beef up error handling here, what if line is not a parseable event?
                    var json = JObject.Parse(line);
                    var eventType = json.Value<string>("event");
                    var timeStamp = json.Value<DateTime?>("timestamp");
                    if (timeStamp.HasValue)
                    {
                        if (timeStamp > lastEntry.GetValueOrDefault(DateTime.MinValue))
                        {
                            lastEntry = timeStamp;
                        }
                    }

                    if (eventType != null && eventType.ToLower() == "fileheader")
                    {
                        fileHeader = JsonConvert.DeserializeObject<FileHeader>(line);
                    }

                    tolerance--;
                }

                if (fileHeader != null)
                {
                    info = new JournalLogFileInfo(filePath, fileHeader.GameVersion, fileHeader.Timestamp);
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Error($"Could not read file {e.FileName} - {e.Message}");
            }
            catch (IOException e)
            {
                Log.Error($"Could not read file {filePath} - {e.Message}");
            }

            return info;
        }

        public IEnumerable<IJournalEntry> ReadAll(DateTimeOffset? since)
        {
            var streamReader = _streamReader.Value;

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
