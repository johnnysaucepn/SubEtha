namespace Howatworks.Assistant.Core
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