using System;
using Howatworks.Matrix.Domain;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Matrix.Core
{
    public class LocationUploader : IUploader<LocationState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationUploader));

        private readonly HttpUploadClient _client;

        public LocationUploader(HttpUploadClient client)
        {
            _client = client;
        }

        public void Upload(GameContext context, LocationState state)
        {
            Log.Info($"Uploading location state '{context.GameVersion}'...");
            Log.Debug(JsonConvert.SerializeObject(state));

            _client.Upload(new Uri($"Api/{context.CommanderName}/{context.GameVersion}/Location", UriKind.Relative), state);
        }
    }
}