using System;
using log4net;
using Newtonsoft.Json;

namespace Thumb.Plugin.SubEtha
{
    public class LocationHttpUploader : IUploader<LocationState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationHttpUploader));

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
            Log.Info($"Uploading location state \"{GameVersion}\"...");
            Log.Debug(JsonConvert.SerializeObject(state));

            _client.Upload(new Uri($"{User}/{GameVersion}/Location", UriKind.Relative), state);
        }
    }
}