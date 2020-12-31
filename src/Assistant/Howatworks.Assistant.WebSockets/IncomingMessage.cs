using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howatworks.Assistant.WebSockets
{
    public class IncomingMessage
    {
        public string SourceSocketId { get; }
        public string MessageType { get; }
        public JToken MessageContent { get; }

        public IncomingMessage(string sourceSocketId, string messageType, JToken messageContent)
        {
            SourceSocketId = sourceSocketId;
            MessageType = messageType;
            MessageContent = messageContent;
        }
    }
}
