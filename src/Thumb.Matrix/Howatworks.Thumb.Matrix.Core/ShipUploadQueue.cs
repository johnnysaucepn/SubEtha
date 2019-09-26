using System;
using Howatworks.Matrix.Domain;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Matrix.Core
{
    public class ShipUploadQueue : UploadQueue<ShipState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ShipUploadQueue));

        public ShipUploadQueue(HttpUploadClient client) : base(client)
        {
        }

        public override void Enqueue(string gameVersion, string cmdrName, ShipState state)
        {
            Log.Info($"Adding ship state '{gameVersion}' to queue");
            Log.Debug(JsonConvert.SerializeObject(state));

            Queue.Enqueue(new Item(gameVersion, cmdrName, state));
        }

        public override void Flush()
        {
            while (Queue.TryDequeue(out var item))
            {
                Client.Upload(new Uri($"Api/{item.CommanderName}/{item.GameVersion}/Ship", UriKind.Relative), item.State);
            }
        }
    }
}
