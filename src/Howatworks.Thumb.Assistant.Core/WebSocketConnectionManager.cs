using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Assistant.Core
{
    public class WebSocketConnectionManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebSocketConnectionManager));

        private static readonly ConcurrentDictionary<Guid, WebSocket> WebSockets = new ConcurrentDictionary<Guid, WebSocket>();

        public event EventHandler<MessageReceivedArgs> MessageReceived = delegate { };
        public event EventHandler<ClientConnectedArgs> ClientConnected = delegate { };

        public async Task Connect(WebSocket socket)
        {
            Guid newConnectionId = Guid.NewGuid();
            if (!WebSockets.TryAdd(newConnectionId, socket))
            {
                throw new Exception("Could not add new websocket connection");
            }

            ClientConnected(this, new ClientConnectedArgs(newConnectionId));

            while (socket.State == WebSocketState.Open)
            {
                var token = CancellationToken.None;
                var buffer = new ArraySegment<byte>(new byte[4096]);
                var received = await socket.ReceiveAsync(buffer, token).ConfigureAwait(false);

                switch (received.MessageType)
                {
                    case WebSocketMessageType.Close:
                        // nothing to do for now...
                        break;

                    case WebSocketMessageType.Text:
                        var incoming = Encoding.UTF8.GetString(buffer.Array ?? throw new InvalidOperationException(), buffer.Offset, buffer.Count);
                        try
                        {
                            MessageReceived.Invoke(this, new MessageReceivedArgs(incoming));
                        }
                        catch (JsonException)
                        {
                            var errorMessage = Encoding.UTF8.GetBytes($"Failed to handle message: {incoming}");
                            await socket.SendAsync(new ArraySegment<byte>(errorMessage), WebSocketMessageType.Text, true, token).ConfigureAwait(false);
                        }
                        break;
                }
            }
            if (socket.State == WebSocketState.Aborted)
            {
                socket.Abort();
            }
        }

        public void DisconnectAll()
        {
            var attempts = 0;
            while (WebSockets.Count > 0 && attempts < 3)
            {
                foreach (var id in WebSockets.Keys)
                {
                    if (WebSockets.TryRemove(id, out var socket))
                    {
                        socket.Abort();
                    }
                }
            }
            if (attempts >= 3)
            {
                throw new Exception("Failed to abort all sockets");
            }
        }

        public async void SendMessageToAllClients(string message)
        {
            var statusBytes = Encoding.UTF8.GetBytes(message);
            foreach (var id in WebSockets.Keys)
            {
                try
                {
                    if (WebSockets.TryGetValue(id, out var socket))
                    {
                        if (socket.State == WebSocketState.Open)
                        {
                            await socket.SendAsync(new ArraySegment<byte>(statusBytes), WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);
                        }
                    }
                }
                catch (WebSocketException e)
                {
                    Log.Error(e);
                }
            }

            Log.Info(message);
        }
    }
}