using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Plugin.Assistant
{
    public class WebSocketConnectionManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebSocketConnectionManager));

        private static readonly ConcurrentBag<WebSocket> WebSockets = new ConcurrentBag<WebSocket>();

        public event EventHandler<MessageReceivedArgs> MessageReceived = delegate { };

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
                            MessageReceived.Invoke(this, new MessageReceivedArgs(incoming));
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

        public async void SendMessageToAllClients(string message)
        {
            var statusBytes = Encoding.UTF8.GetBytes(message);
            foreach (var socket in WebSockets)
            {
                try
                {
                    await socket.SendAsync(new ArraySegment<byte>(statusBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (WebSocketException e)
                {
                    Log.Error(e);
                }
            }

            Console.WriteLine(message);
        }
    }
}