namespace Howatworks.Assistant.Core.Messages
{
    public class StartActivateBindingMessage : IAssistantMessage
    {
        public AssistantMessageType MessageType => AssistantMessageType.StartActivateBinding;

        public string BindingName { get; set; }
    }
}