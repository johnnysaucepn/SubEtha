using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketManager
{
    public abstract class WebSocketHandler
    {
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketHandler"/> class.
        /// </summary>
        /// <param name="webSocketConnectionManager">The web socket connection manager.</param>
        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        /// <summary>
        /// Called when a client has connected to the server.
        /// </summary>
        /// <param name="socket">The web-socket of the client.</param>
        /// <returns>Awaitable Task.</returns>
        public virtual async Task OnConnected(WebSocket socket)
        {
            WebSocketConnectionManager.AddSocket(socket);

            // Confirmation sent to client
            var id = WebSocketConnectionManager.GetId(socket);
            await SendAsync(socket, id).ConfigureAwait(false);
        }

        /// <summary>
        /// Called when a client has disconnected from the server.
        /// </summary>
        /// <param name="socket">The web-socket of the client.</param>
        /// <returns>Awaitable Task.</returns>
        public virtual async Task OnDisconnected(WebSocket socket)
        {
            var id = WebSocketConnectionManager.GetId(socket);
            await WebSocketConnectionManager.RemoveSocket(id).ConfigureAwait(false);
        }

        public async Task SendAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            var encodedMessage = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(buffer: new ArraySegment<byte>(array: encodedMessage,
                                                                  offset: 0,
                                                                  count: encodedMessage.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        public async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string receivedMessage)
        {
            // pass in some form of Action<string> handler
        }

    }
}