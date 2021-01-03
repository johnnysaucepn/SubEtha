namespace Howatworks.Assistant.Core.Messages
{
    public interface IAssistantMessage
    {
        AssistantMessageType MessageType { get; }
    }
}