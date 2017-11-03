using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Thumb.Plugin.SubEtha
{
    public class ShipHttpUploader : IUploader<ShipState>
    {
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
            Trace.TraceInformation($"Uploading ship state \"{GameVersion}\"...");
            Debug.WriteLine(JsonConvert.SerializeObject(state));

            _client.Upload(new Uri($"{User}/{GameVersion}/Ship", UriKind.Relative), state);
        }
    }
}
