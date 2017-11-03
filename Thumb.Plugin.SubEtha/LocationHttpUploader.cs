using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Thumb.Plugin.SubEtha
{
    public class LocationHttpUploader : IUploader<LocationState>
    {
        private readonly HttpUploadClient _client;
        public string User { get; }
        public string GameVersion { get; }

        public LocationHttpUploader(string user, string gameVersion, HttpUploadClient client)
        {
            User = user;
            GameVersion = gameVersion;
            _client = client;
        }

        public void Upload(LocationState state)
        {
            Trace.TraceInformation($"Uploading location state \"{GameVersion}\"...");
            Debug.WriteLine(JsonConvert.SerializeObject(state));

            _client.Upload(new Uri($"{User}/{GameVersion}/Location", UriKind.Relative), state);
        }
    }
}