using System.Collections.Generic;
using System.IO;
using System.Linq;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Parser;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.SubEtha.Monitor
{
    public class NewLiveJournalMonitor : INewJournalLineSource
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NewLiveJournalMonitor));

        private readonly List<NewLiveJournalReader> _liveReaders;

        public NewLiveJournalMonitor(IConfiguration config, INewJournalReaderFactory readerFactory)
        {
            var folder = config["JournalFolder"];
            var liveFilenames = config["RealTimeFilenames"].Split(';').Select(x => x.Trim());

            _liveReaders = new List<NewLiveJournalReader>();
            foreach (var filename in liveFilenames)
            {
                var file = new FileInfo(Path.Combine(folder, filename));
                var newReader = readerFactory.CreateLiveJournalReader(file);
                _liveReaders.Add(newReader);
            }
        }

        public IEnumerable<NewJournalLine> GetJournalLines()
        {
            return _liveReaders
                .Select(x => x.ReadCurrent())
                .Where(l => l != null);

        }
    }
}
