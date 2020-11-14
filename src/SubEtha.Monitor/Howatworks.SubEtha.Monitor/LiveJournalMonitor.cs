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
    public class LiveJournalMonitor : IJournalLineSource
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LiveJournalMonitor));

        private readonly List<LiveJournalReader> _liveReaders;

        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;

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
                JournalFileWatchingStarted?.Invoke(this, new JournalFileEventArgs(file));
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
