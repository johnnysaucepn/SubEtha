using System.Collections.Generic;
using Howatworks.SubEtha.Parser;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class MatrixJournalProcessor : IJournalProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixJournalProcessor));

        private readonly IList<IJournalProcessor> _childProcessors = new List<IJournalProcessor>();

        public MatrixJournalProcessor(IConfiguration config, string user, string gameVersion, JournalEntryRouter router, HttpUploadClient client)
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