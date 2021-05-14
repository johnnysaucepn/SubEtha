using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Parser;
using Microsoft.Extensions.Configuration;

namespace Howatworks.SubEtha.Monitor
{
    public class LiveJournalMonitor : IJournalLineSource
    {
        private readonly List<LiveJournalReader> _liveReaders;

        private readonly ISubject<JournalWatchActivity> _journalFileWatch = new Subject<JournalWatchActivity>();

        public IObservable<JournalWatchActivity> JournalFileWatch => _journalFileWatch.AsObservable();

        public LiveJournalMonitor(IConfiguration config, IJournalReaderFactory readerFactory)
        {
            var folder = config["JournalFolder"];
            var liveFilenames = config["RealTimeFilenames"].Split(';').Select(x => x.Trim());

            _liveReaders = new List<LiveJournalReader>();
            foreach (var filename in liveFilenames)
            {
                var file = new FileInfo(Path.Combine(folder, filename));
                var newReader = readerFactory.CreateLiveJournalReader(file);
                _liveReaders.Add(newReader);
                _journalFileWatch.OnNext(new JournalWatchActivity(JournalWatchAction.Started, file));
            }
        }

        public IEnumerable<JournalLine> GetJournalLines()
        {
            return _liveReaders
                .Select(x => x.ReadCurrent())
                .Where(l => l != null);
        }
    }
}
