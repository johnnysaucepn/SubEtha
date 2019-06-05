namespace Howatworks.Thumb.Plugin.Assistant
{
    public class MessageReceivedArgs
    {
        public readonly string Message;

        public MessageReceivedArgs(string rawMessage)
        {
            Message = rawMessage;
        }
    }
}