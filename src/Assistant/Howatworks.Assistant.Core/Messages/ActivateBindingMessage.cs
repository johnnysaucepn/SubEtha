namespace Howatworks.Assistant.Core.Messages
{
    public class ActivateBindingMessage : IAssistantMessage
    {
        public AssistantMessageType MessageType => AssistantMessageType.ActivateBinding;

        public string BindingName { get; set; }
    }
}