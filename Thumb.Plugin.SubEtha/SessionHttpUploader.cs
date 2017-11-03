using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Thumb.Plugin.SubEtha
{
    public class SessionHttpUploader : IUploader<SessionState>
    {
        private readonly HttpUploadClient _client;
        public string User { get; }
        public string GameVersion { get; }

        public SessionHttpUploader(string user, string gameVersion, HttpUploadClient client)
        {
            User = user;
            GameVersion = gameVersion;
            _client = client;
        }

        public void Upload(SessionState state)
        {
            Trace.TraceInformation($"Uploading session state \"{GameVersion}\"...");
            Debug.WriteLine(JsonConvert.SerializeObject(state));

            _client.Upload(new Uri($"{User}/{GameVersion}/Session", UriKind.Relative), state);
        }


    }
}
