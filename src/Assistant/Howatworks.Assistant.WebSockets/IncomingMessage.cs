namespace Howatworks.Assistant.WebSockets
{
    public class IncomingMessage
    {
        public string SourceSocketId { get; }
        public string Message { get; }

        public IncomingMessage(string sourceSocketId, string message)
        {
            SourceSocketId = sourceSocketId;
            Message = message;
        }
    }
}
