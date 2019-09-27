using System;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class LocationUploadQueue : UploadQueue<LocationState>
    {

        public LocationUploadQueue(HttpUploadClient client) : base(client)
        {
        }

        protected override Uri BuildUri(Item item)
        {
            return new Uri($"Api/{item.CommanderName}/{item.GameVersion}/Location", UriKind.Relative);
        }
    }
}