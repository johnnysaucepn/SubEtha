using System;

namespace Howatworks.Assistant.Core
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