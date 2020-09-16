using Howatworks.SubEtha.Journal;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Howatworks.SubEtha.Parser
{

    public class NewLogJournalReader : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NewLogJournalReader));

        public FileInfo File { get; }

        public NewJournalLogFileInfo Context { get; }

        private Lazy<StreamReader> _stream;
        private bool disposedValue;
        private readonly IJournalParser _parser;

        //public IObservable<NewJournalLine> JournalEntries { get; internal set; }

        public NewLogJournalReader(FileInfo file, IJournalParser parser)
        {
            File = file;
            _parser = parser;
            _stream = new Lazy<StreamReader>(GetStreamReader);

            //JournalEntries = Observable.Interval(TimeSpan.FromSeconds(5)).SelectMany(_ => ReadNext());
            Context = ReadFileInfo();
        }

        private StreamReader GetStreamReader()
        {
            var fileStream = new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new StreamReader(fileStream);
        }

        private NewJournalLogFileInfo ReadFileInfo()
        {
            FileHeader fileHeader = null;
            var info = new NewJournalLogFileInfo(File);
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

                            if (eventType?.Equals("fileheader", StringComparison.InvariantCultureIgnoreCase) == true)
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
                    info = new NewJournalLogFileInfo(File, fileHeader.GameVersion, fileHeader.Timestamp);
                }
            }
            catch (FileNotFoundException e)
            {
                Log.Error($"Could not read file '{e.FileName}' - {e.Message}");
            }
            catch (IOException e)
            {
                Log.Error($"Could not read file '{File.FullName}' - {e.Message}");
            }

            return info;
        }

        public IObservable<NewJournalLine> GetObservable()
        {
            return Observable.Create<NewJournalLine>(observer =>
            {
                foreach (var y in ReadNext())
                {
                    observer.OnNext(y);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        public IEnumerable<NewJournalLine> ReadNext()
        {
            var streamReader = _stream.Value;

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                Log.Debug(line);
                if (string.IsNullOrWhiteSpace(line)) continue;

                yield return new NewJournalLine(Context, line);
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
