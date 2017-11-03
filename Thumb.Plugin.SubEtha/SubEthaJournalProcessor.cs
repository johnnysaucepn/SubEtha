using System.Collections.Generic;
using System.Diagnostics;
using Howatworks.Configuration;
using Howatworks.PlayerJournal;
using Howatworks.PlayerJournal.Processing;
using Newtonsoft.Json;

namespace Thumb.Plugin.SubEtha
{
    public class SubEthaJournalProcessor : IJournalProcessor
    {
        private readonly IList<IJournalProcessor> _childProcessors = new List<IJournalProcessor>();

        public SubEthaJournalProcessor(IConfigReader config, string user, string gameVersion)
        {
            var client = new HttpUploadClient(config);
            _childProcessors.Add(new LocationManager(new LocationHttpUploader(user, gameVersion, client)));
            _childProcessors.Add(new SessionManager(new SessionHttpUploader(user, gameVersion, client)));
            _childProcessors.Add(new ShipManager(new ShipHttpUploader(user, gameVersion, client)));
            //_childProcessors.Add(new LocationManager(new DummyUploader<LocationState>()));
            //_childProcessors.Add(new SessionManager(new DummyUploader<SessionState>()));
            //_childProcessors.Add(new ShipManager(new DummyUploader<ShipState>()));
        }

        /// <summary>
        /// By this point we already know that the entry is targetted to this game version
        /// </summary>
        /// <param name="entry"></param>
        public bool Apply(JournalEntryBase entry)
        {
            var somethingApplied = false;
            Debug.WriteLine(JsonConvert.SerializeObject(entry));

            foreach (var manager in _childProcessors)
            {
                if (manager.Apply(entry))
                {
                    somethingApplied = true;
                }
            }
            if (!somethingApplied)
            {
                Trace.TraceInformation($"No handler applied for event type {entry.Event}");
            }
            return somethingApplied;

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