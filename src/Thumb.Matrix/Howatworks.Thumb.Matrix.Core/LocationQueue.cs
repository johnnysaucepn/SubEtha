using System;
using System.Collections.Concurrent;
using Howatworks.Matrix.Domain;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Matrix.Core
{
    public class LocationUploadQueue : UploadQueue<LocationState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationUploadQueue));

        public LocationUploadQueue(HttpUploadClient client) : base(client)
        {
        }

        public override void Enqueue(string gameVersion, string cmdrName, LocationState state)
        {
            Log.Info($"Adding location state '{gameVersion}' to queue");
            Log.Debug(JsonConvert.SerializeObject(state));

            Queue.Enqueue(new Item(gameVersion, cmdrName, state));
        }

        public override void Flush()
        {
            while (Queue.TryDequeue(out var item))
            {
                Client.Upload(new Uri($"Api/{item.CommanderName}/{item.GameVersion}/Location", UriKind.Relative), item.State);
            }
        }
    }
}