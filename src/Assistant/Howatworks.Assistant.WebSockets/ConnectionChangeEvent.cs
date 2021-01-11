using log4net;
using System;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

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
