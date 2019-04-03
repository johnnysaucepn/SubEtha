namespace Thumb.Plugin.Controller
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