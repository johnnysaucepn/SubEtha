using System;
using Howatworks.Matrix.Domain;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class ShipUploader : IUploader<ShipState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ShipUploader));

        private readonly HttpUploadClient _client;

        public ShipUploader(HttpUploadClient client)
        {
            _client = client;
        }

        public void Upload(GameContext context, ShipState state)
        {
            Log.Info($"Uploading ship state \"{context.GameVersion}\"...");
            Log.Debug(JsonConvert.SerializeObject(state));

            _client.Upload(new Uri($"Api/{context.CommanderName}/{context.GameVersion}/Ship", UriKind.Relative), state);
        }
    }
}
