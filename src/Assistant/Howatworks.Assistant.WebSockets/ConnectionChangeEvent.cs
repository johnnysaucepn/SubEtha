namespace Howatworks.Assistant.WebSockets
{
    public class ConnectionChangeEvent
    {
        public string SocketId { get; }
        public ConnectionChange Change {get;}

        public ConnectionChangeEvent(string socketId, ConnectionChange change)
        {
            SocketId = socketId;
            Change = change;
        }
    }
}
