using System;

namespace Howatworks.Thumb.Plugin.Assistant
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