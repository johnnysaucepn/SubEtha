﻿using System;
using System.Collections.Generic;
using System.IO;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Parser.Logging;

namespace Howatworks.SubEtha.Parser
{
    public class LogJournalReader : IDisposable
    {
        private static readonly SubEthaLog Log = SubEthaLog.GetLogger<LogJournalReader>();

        public FileInfo File { get; }

        public JournalLogFileInfo Context { get; }

        private Lazy<StreamReader> _stream;
        private bool disposedValue;
        private readonly IJournalParser _parser;

        public LogJournalReader(FileInfo file, IJournalParser parser)
        {
            File = file;
            _parser = parser;
            _stream = new Lazy<StreamReader>(GetStreamReader);

            Context = ReadFileInfo();
        }

        private StreamReader GetStreamReader()
        {
            var fileStream = new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new StreamReader(fileStream);
        }

        private JournalLogFileInfo ReadFileInfo()
        {
            FileHeader fileHeader = null;
            var info = new JournalLogFileInfo(File);
            DateTimeOffset? lastEntry = null;

            try
            {
                // Get a new reader for this action only
                using (var streamReader = GetStreamReader())
                {
                    // FileHeader *should* be the first line in the file, but at least try the first 5
                    var tolerance = 5;

                    while (!streamReader.EndOfStream && tolerance > 0 && fileHeader == null)
                    {
                        var line = streamReader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        try
                        {
                            var (eventType, timestamp) = _parser.ParseCommonProperties(line);

                            if (timestamp > (lastEntry ?? DateTimeOffset.MinValue))
                            {
                                lastEntry = timestamp;
                            }

                            if (eventType?.Equals(nameof(FileHeader), StringComparison.InvariantCultureIgnoreCase) == true)
                            {
                                fileHeader = _parser.Parse<FileHeader>(line);
                            }
                        }
                        catch (JournalParseException)
                        {
                            // Cannot parse line, don't consider as potential fileheader
                        }

                        tolerance--;
                    }
                }

                if (fileHeader != null)
                {
                    info = new JournalLogFileInfo(File, fileHeader.GameVersion, fileHeader.Timestamp, lastEntry ?? fileHeader.Timestamp);
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Error($"Could not read file '{e.FileName}'", e);
            }
            catch (IOException e)
            {
                Log.Error($"Could not read file '{File.FullName}'", e);
            }

            return info;
        }

        public IEnumerable<JournalLine> ReadLines()
        {
            var streamReader = _stream.Value;

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                Log.Debug(line);
                if (string.IsNullOrWhiteSpace(line)) continue;

                yield return new JournalLine(Context, line);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_stream.IsValueCreated) _stream.Value.Dispose();
                    _stream = null;
                }
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~IncrementalJournalReader()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
