using System;
using System.Collections.Generic;
using System.IO;
using Howatworks.SubEtha.Journal;
using log4net;

namespace Howatworks.SubEtha.Parser
{
    public class IncrementalJournalReader : IJournalReader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(IncrementalJournalReader));

        private readonly IJournalParser _parser;
        private readonly Lazy<StreamReader> _streamReader;
        public string FilePath { get; }
        private JournalLogFileInfo FileInfo { get; }
        public DateTimeOffset? LastEntryTimeStamp { get; private set; }

        public IncrementalJournalReader(string filePath, IJournalParser parser)
        {
            _parser = parser;
            FilePath = filePath;
            FileInfo = ReadFileInfo();
            _streamReader = new Lazy<StreamReader>(GetStreamReader);
        }

        private StreamReader GetStreamReader()
        {
            var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new StreamReader(fileStream);
        }

        public bool FileExists => File.Exists(FileInfo.Path);

        private JournalLogFileInfo ReadFileInfo()
        {
            FileHeader fileHeader = null;
            var info = new JournalLogFileInfo(FilePath);
            DateTimeOffset? lastEntry = null;

            try
            {
                var streamReader = GetStreamReader();

                // FileHeader *should* be the first line in the file, but at least try the first 5
                var tolerance = 5;

                while (!streamReader.EndOfStream && tolerance > 0 && fileHeader == null)
                {
                    var line = streamReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // TODO: beef up error handling here, what if line is not a parseable event?
                    var (eventType, timestamp) = _parser.ParseCommonProperties(line);

                    if (timestamp > lastEntry.GetValueOrDefault(DateTimeOffset.MinValue))
                    {
                        lastEntry = timestamp;
                    }

                    if (eventType != null && eventType.Equals("fileheader", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fileHeader = _parser.Parse<FileHeader>(line);
                    }

                    tolerance--;
                }

                if (fileHeader != null)
                {
                    info = new JournalLogFileInfo(FilePath, fileHeader.GameVersion, fileHeader.Timestamp);
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Error($"Could not read file {e.FileName} - {e.Message}");
            }
            catch (IOException e)
            {
                Log.Error($"Could not read file {FilePath} - {e.Message}");
            }

            return info;
        }

        public IEnumerable<IJournalEntry> ReadAll(DateTimeOffset since)
        {
            var streamReader = _streamReader.Value;

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                Log.Debug(line);
                if (string.IsNullOrWhiteSpace(line)) continue;

                // TODO: beef up error handling here, what if line is not a parseable event?
                var (eventType, timestamp) = _parser.ParseCommonProperties(line);

                if (timestamp <= since) continue;

                IJournalEntry journalEntry = null;
                try
                {
                    journalEntry = _parser.Parse(eventType, line);
                }
                catch (JournalParseException e)
                {
                    Log.Error($"'{FilePath}': {e.JournalFragment}");
                    Log.Error(e.Message);
                }
                catch (UnrecognizedJournalException e)
                {
                    Log.Warn($"'{FilePath}': {e.JournalFragment}");
                    Log.Warn(e.Message);
                }

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
