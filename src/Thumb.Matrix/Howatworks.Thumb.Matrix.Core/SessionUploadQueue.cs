using System;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class SessionUploadQueue : UploadQueue<SessionState>
    {

        public SessionUploadQueue(HttpUploadClient client) : base(client)
        {
        }

        protected override Uri BuildUri(Item item)
        {
            return new Uri($"Api/{item.CommanderName}/{item.GameVersion}/Session", UriKind.Relative);
        }
    }
}
