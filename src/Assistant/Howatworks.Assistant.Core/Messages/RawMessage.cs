namespace Howatworks.Assistant.Core.Messages
{
    public class RawMessage : IAssistantMessage
    {
        public AssistantMessageType MessageType => AssistantMessageType.Unknown;

        public string MessageContent { get; set; }
    }
}
