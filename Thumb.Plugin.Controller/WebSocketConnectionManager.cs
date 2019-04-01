using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Thumb.Plugin.Controller
{
    public class WebSocketConnectionManager
    {
        private static readonly ConcurrentBag<WebSocket> WebSockets = new ConcurrentBag<WebSocket>();

        private readonly StatusManager _statusManager;

        public WebSocketConnectionManager(StatusManager statusManager)
        {
            _statusManager = statusManager;
        }

        public async Task Connect(WebSocket socket)
        {
            WebSockets.Add(socket);

            while (socket.State == WebSocketState.Open)
            {
                var token = CancellationToken.None;
                var buffer = new ArraySegment<byte>(new byte[4096]);
                var received = await socket.ReceiveAsync(buffer, token);

                switch (received.MessageType)
                {
                    case WebSocketMessageType.Close:
                        // nothing to do for now...
                        break;

                    case WebSocketMessageType.Text:
                        var incoming = Encoding.UTF8.GetString(buffer.Array ?? throw new InvalidOperationException(), buffer.Offset, buffer.Count);
                        try
                        {
                            var structuredMessage = JsonConvert.DeserializeObject<ControlRequest>(incoming);
                            _statusManager.ActivateBinding(structuredMessage);

                        }
                        catch (JsonException)
                        {
                            var errorMessage = Encoding.UTF8.GetBytes($"Failed to handle message: {incoming}");
                            await socket.SendAsync(new ArraySegment<byte>(errorMessage), WebSocketMessageType.Text, true, token);
                        }
                        break;
                }
            }
        }

        public void DisconnectAll()
        {
            while (WebSockets.TryTake(out var socket))
            {
                socket.Abort();
            }
        }

        public async void SendStatusToAllClients(GameStatus status)
        {
            var statusMessage = JsonConvert.SerializeObject(status, Formatting.Indented);

            var statusBytes = Encoding.UTF8.GetBytes(statusMessage);
            foreach (var socket in WebSockets)
            {
                await socket.SendAsync(new ArraySegment<byte>(statusBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            Console.WriteLine(statusMessage);
        }
    }
}