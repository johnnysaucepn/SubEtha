using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Parser;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.SubEtha.Monitor
{
    public class NewLogJournalMonitor : INewJournalLineSource
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NewLogJournalMonitor));

        private readonly CustomFileWatcher _logFileWatcher;
        private readonly SortedList<DateTimeOffset, NewLogJournalReader> _logReaders;

        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        public NewLogJournalMonitor(IConfiguration config, INewJournalReaderFactory readerFactory, DateTimeOffset startTime)
        {
            var folder = config["JournalFolder"];
            var logPattern = config["JournalPattern"];

            _logReaders = new SortedList<DateTimeOffset, NewLogJournalReader>();

            _logFileWatcher = new CustomFileWatcher(folder, logPattern);

            _logFileWatcher.CreatedFiles.Subscribe(f =>
            {
                var file = new FileInfo(Path.Combine(folder, f));
                var newReader = readerFactory.CreateLogJournalReader(file);

                // As long as the files has some entries that happened after our desired start time, it's a valid source
                if (newReader.Context.LastEntry >= startTime)
                {
                    _logReaders.Add(newReader.Context.HeaderTimestamp, newReader);
                    JournalFileWatchingStarted?.Invoke(this, new JournalFileEventArgs(f)); // TODO: use FileInfo instead?
                }
            });

            _logFileWatcher.DeletedFiles.Subscribe(f =>
            {
                foreach (var reader in _logReaders.Where(x => x.Value.File.Name.Equals(f)))
                {
                    _logReaders.Remove(reader.Key);
                    JournalFileWatchingStopped?.Invoke(this, new JournalFileEventArgs(f)); // TODO: use FileInfo instead?
                }
            });
            _logFileWatcher.Start();
        }

        public IEnumerable<NewJournalLine> GetJournalLines()
        {
            return _logReaders
                .SelectMany(x => x.Value.ReadLines())
                .Where(l => l != null);
        }
    }
}
