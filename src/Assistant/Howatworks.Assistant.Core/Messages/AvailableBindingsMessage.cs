using System.Collections.Generic;

namespace Howatworks.Assistant.Core.Messages
{
    internal class AvailableBindingsMessage : IAssistantMessage
    {
        public AssistantMessageType MessageType => AssistantMessageType.AvailableBindings;

        public IReadOnlyCollection<string> Bindings { get; set; }

        public AvailableBindingsMessage(IReadOnlyCollection<string> bindings)
        {
            Bindings = bindings;
        }
    }
}