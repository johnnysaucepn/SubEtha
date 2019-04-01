using System.Collections.Generic;
using Howatworks.PlayerJournal.Parser;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Thumb.Plugin.SubEtha
{
    public class SubEthaJournalProcessor : IJournalProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SubEthaJournalProcessor));

        private readonly IList<IJournalProcessor> _childProcessors = new List<IJournalProcessor>();

        public SubEthaJournalProcessor(IConfiguration config, string user, string gameVersion, JournalEntryRouter router, HttpUploadClient client)
        {
            _childProcessors.Add(new LocationManager(router, new LocationHttpUploader(user, gameVersion, client)));
            _childProcessors.Add(new SessionManager(router, new SessionHttpUploader(user, gameVersion, client)));
            _childProcessors.Add(new ShipManager(router, new ShipHttpUploader(user, gameVersion, client)));
            //_childProcessors.Add(new LocationManager(new DummyUploader<LocationState>()));
            //_childProcessors.Add(new SessionManager(new DummyUploader<SessionState>()));
            //_childProcessors.Add(new ShipManager(new DummyUploader<ShipState>()));
        }

        public void Flush()
        {
            foreach (var manager in _childProcessors)
            {
                manager.Flush();
            }
        }
    }
}