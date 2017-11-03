using System;
using System.Collections.Generic;
using System.IO;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Howatworks.PlayerJournal.Parser
{
    internal class JournalReader
    {
        private static ILog Log = LogManager.GetLogger<JournalReader>();

        private readonly IJournalParser _parser;
        private StreamReader _streamReader;
        public JournalFileInfo FileInfo { get; }

        public JournalReader(string filePath, IJournalParser parser)
        {
            _parser = parser;
            FileInfo = ReadFileInfo(filePath);
        }
        private static StreamReader GetStreamReader(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new StreamReader(fileStream);
        }

        public bool FileExists => File.Exists(FileInfo.Path);

        private static JournalFileInfo ReadFileInfo(string filePath)
        {
            FileHeader fileHeader = null;
            var info = new JournalFileInfo(filePath);
            DateTime? lastEntry = null;

            using (var streamReader = GetStreamReader(filePath))
            {
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
            }
            if (fileHeader != null)
            {
                info = new JournalFileInfo(filePath, fileHeader.GameVersion, fileHeader.TimeStamp, lastEntry.GetValueOrDefault(fileHeader.TimeStamp));
            }
            return info;
        }

        public IEnumerable<JournalEntryBase> ReadAll(DateTime? since)
        {
            // Re-use reader where possible
            if (_streamReader == null)
            {
                _streamReader = GetStreamReader(FileInfo.Path);
            }
            while (!_streamReader.EndOfStream)
            {
                var line = _streamReader.ReadLine();
                Log.Trace(line);
                if (string.IsNullOrWhiteSpace(line)) continue;

                // TODO: beef up error handling here, what if line is not a parseable event?
                var json = JObject.Parse(line);
                var timestamp = json.Value<DateTime>("timestamp");

                if (timestamp <= since.GetValueOrDefault(DateTime.MinValue)) continue;

                var eventType = json.Value<string>("event");

                var entry = _parser.Parse(FileInfo.GameVersion, eventType, line);

                // TODO: remove this check once we're confident we should recognise all types
                if (entry != null)
                {
                    yield return entry;
                }
            }
        }
    }
}
