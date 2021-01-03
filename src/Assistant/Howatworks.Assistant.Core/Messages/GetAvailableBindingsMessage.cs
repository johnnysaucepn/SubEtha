namespace Howatworks.Assistant.Core.Messages
{
    public class GetAvailableBindingsMessage : IAssistantMessage
    {
        public AssistantMessageType MessageType => AssistantMessageType.GetAvailableBindings;
    }
}