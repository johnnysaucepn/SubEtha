using System.Collections.Generic;
using System.Diagnostics;
using Howatworks.Configuration;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Processing;
using Howatworks.PlayerJournal.Serialization;
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
        /// <param name="journalEntry"></param>
        public bool Apply(IJournalEntry journalEntry)
        {
            var somethingApplied = false;
            Debug.WriteLine(JsonConvert.SerializeObject(journalEntry));

            foreach (var manager in _childProcessors)
            {
                if (manager.Apply(journalEntry))
                {
                    somethingApplied = true;
                }
            }
            if (!somethingApplied)
            {
                Trace.TraceInformation($"No handler applied for event type {journalEntry.Event}");
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