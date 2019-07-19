using System;
using Howatworks.Matrix.Domain;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class SessionUploader : IUploader<SessionState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SessionUploader));

        private readonly HttpUploadClient _client;

        public SessionUploader(HttpUploadClient client)
        {
            _client = client;
        }

        public void Upload(GameContext context, SessionState state)
        {
            Log.Info($"Uploading session state \"{context.GameVersion}\"...");
            Log.Debug(JsonConvert.SerializeObject(state));

            _client.Upload(new Uri($"{context.CommanderName}/{context.GameVersion}/Session", UriKind.Relative), state);
        }


    }
}
