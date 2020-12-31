using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howatworks.Assistant.WebSockets
{

    public class OutgoingMessage
    {
        public string MessageType { get; }
        public JToken MessageContent { get; }

        public OutgoingMessage(string messageType, JToken messageContent)
        {
            MessageType = messageType;
            MessageContent = messageContent;
        }

    }
}
