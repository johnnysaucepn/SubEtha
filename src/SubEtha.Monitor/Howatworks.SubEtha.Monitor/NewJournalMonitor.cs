using Howatworks.SubEtha.Parser;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Howatworks.SubEtha.Monitor
{

    public class NewJournalMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NewJournalMonitor));

        private readonly IJournalParser _parser;

        private readonly List<NewLiveJournalReader> _liveReaders;

        private readonly CustomFileWatcher _journalFileWatcher;
        private readonly SortedList<DateTimeOffset, NewLogJournalReader> _logReaders;

        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        public NewJournalMonitor(IConfiguration config, IJournalParser parser)
        {
            _parser = parser;

            var folder = config["JournalFolder"];
            var pattern = config["JournalPattern"];
            var liveFilenames = config["RealTimeFilenames"].Split(';').Select(x => x.Trim());

            _logReaders = new SortedList<DateTimeOffset, NewLogJournalReader>();

            _journalFileWatcher = new CustomFileWatcher(folder, pattern);
            _journalFileWatcher.CreatedFiles.Subscribe(f =>
            {
                var file = new FileInfo(Path.Combine(folder, f));
                var newReader = new NewLogJournalReader(file, _parser);

                // TODO: wrap the add/remove mechanics in a new class
                _logReaders.Add(newReader.Context.HeaderTimestamp, newReader);
                JournalFileWatchingStarted?.Invoke(this, new JournalFileEventArgs(f)); // TODO: use FileInfo instead?
            });
            _journalFileWatcher.DeletedFiles.Subscribe(f =>
            {
                foreach (var reader in _logReaders.Where(x => x.Value.File.Name.Equals(f)))
                {
                    // TODO: wrap the add/remove mechanics in a new class
                    _logReaders.Remove(reader.Key);
                    JournalFileWatchingStopped?.Invoke(this, new JournalFileEventArgs(f)); // TODO: use FileInfo instead?
                }
            });
            _journalFileWatcher.Start();

            _liveReaders = new List<NewLiveJournalReader>();
            foreach (var filename in liveFilenames)
            {
                var file = new FileInfo(Path.Combine(folder, filename));
                var newReader = new NewLiveJournalReader(file);
                _liveReaders.Add(newReader);
            }
        }

        public IObservable<NewJournalLine> GetJournalEntries()
        {
            return _liveReaders.Select(x => x.GetObservable())
                .Union(_logReaders.Select(x => x.Value.GetObservable()))
                .Merge();
        }

    }
}
