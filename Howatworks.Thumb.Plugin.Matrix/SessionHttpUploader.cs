using System;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class SessionHttpUploader : IUploader<SessionState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SessionHttpUploader));

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
            Log.Info($"Uploading session state \"{GameVersion}\"...");
            Log.Debug(JsonConvert.SerializeObject(state));

            _client.Upload(new Uri($"{User}/{GameVersion}/Session", UriKind.Relative), state);
        }


    }
}
