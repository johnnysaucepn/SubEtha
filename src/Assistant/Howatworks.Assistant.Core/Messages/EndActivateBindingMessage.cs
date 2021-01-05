namespace Howatworks.Assistant.Core.Messages
{
    public class EndActivateBindingMessage : IAssistantMessage
    {
        public AssistantMessageType MessageType => AssistantMessageType.EndActivateBinding;

        public string BindingName { get; set; }
    }
}