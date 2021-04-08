using System.Collections.Generic;

namespace Howatworks.Assistant.Core.Messages
{
    internal class AvailableBindingsMessage : IAssistantMessage
    {
        public AssistantMessageType MessageType => AssistantMessageType.AvailableBindings;

        public IEnumerable<string> Bindings { get; set; }

        public AvailableBindingsMessage(IEnumerable<string> bindings)
        {
            Bindings = bindings;
        }
    }
}