using System;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class ShipUploadQueue : UploadQueue<ShipState>
    {

        public ShipUploadQueue(HttpUploadClient client) : base(client)
        {
        }

        protected override Uri BuildUri(Item item)
        {
            return new Uri($"Api/{item.CommanderName}/{item.GameVersion}/Ship", UriKind.Relative);
        }
    }
}
