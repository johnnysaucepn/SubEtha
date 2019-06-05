using System;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class ShipHttpUploader : IUploader<ShipState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ShipHttpUploader));

        private readonly HttpUploadClient _client;
        public string User { get; }
        public string GameVersion { get; }

        public ShipHttpUploader(string user, string gameVersion, HttpUploadClient client)
        {
            User = user;
            GameVersion = gameVersion;
            _client = client;
        }

        public void Upload(ShipState state)
        {
            Log.Info($"Uploading ship state \"{GameVersion}\"...");
            Log.Debug(JsonConvert.SerializeObject(state));

            _client.Upload(new Uri($"{User}/{GameVersion}/Ship", UriKind.Relative), state);
        }
    }
}
