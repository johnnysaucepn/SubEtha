using System;

namespace Howatworks.Assistant.Core
{
    public class ClientConnectionChange
    {
        public Guid ClientId { get; }
        public ClientConnectionState NewState { get; }

        public ClientConnectionChange(ClientConnectionState state, Guid clientId)
        {
            NewState = state;
            ClientId = clientId;
        }
    }
}