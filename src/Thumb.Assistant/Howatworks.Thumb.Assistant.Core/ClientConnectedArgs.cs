using System;

namespace Howatworks.Thumb.Assistant.Core
{
    public class ClientConnectedArgs : EventArgs
    {
        public Guid ClientId { get; set; }

        public ClientConnectedArgs(Guid newConnectionId)
        {
            ClientId = newConnectionId;
        }
    }
}