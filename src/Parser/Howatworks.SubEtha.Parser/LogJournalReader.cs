using System;
using System.Collections.Generic;
using System.IO;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Parser
{
    public class LogJournalReader : IJournalReader, IDisposable
    {
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
            // Get a new reader for this action only
            using (var streamReader = GetStreamReader())
            {
                // FileHeader *should* be the first line in the file, but at least try the first 5
                var tolerance = 5;

                while (!streamReader.EndOfStream && tolerance > 0)
                {
                    var line = streamReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var fileHeaderParsed = _parser.Parse<FileHeader>(line);
                    if (fileHeaderParsed.IsSuccess)
                    {
                        var fileHeader = fileHeaderParsed.Value;
                        return new JournalLogFileInfo(File, fileHeader.GameVersion, fileHeader.Timestamp);
                    }
                    tolerance--;
                }
            }

            return new JournalLogFileInfo(File);
        }

        public IEnumerable<JournalLine> ReadAll()
        {
            var streamReader = _stream.Value;

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                yield return new JournalLine(Context, line);
            }
        }

        public JournalResult<JournalLine> ReadNext()
        {
            var streamReader = _stream.Value;

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                return JournalResult.Success(new JournalLine(Context, line));
            }
            return JournalResult.Failure<JournalLine>($"End of file '{File.Name}' reached");
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
        // ~LogJournalReader()
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
