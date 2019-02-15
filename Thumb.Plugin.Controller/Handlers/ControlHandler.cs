using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketManager;

namespace Thumb.Plugin.Controller.Handlers
{
    public class ControlHandler : WebSocketHandler
    {
        public ControlHandler(WebSocketConnectionManager webSocketConnectionManager)
            : base(webSocketConnectionManager)
        {

        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);

            var message = $"{socketId} is now connected";

            //await SendAsync(socket, message);
            Console.WriteLine(message);
        }

        // this method can be called from a client, doesn't return anything.
        public async Task SendMessage(WebSocket socket, string message)
        {
            try
            {
                var structuredMessage = JsonConvert.DeserializeObject<ControlRequest>(message);
                PretendToPressKey(socket, structuredMessage);

            }
            catch (JsonException)
            {
                await SendAsync(socket, message.ToUpperInvariant());
            }

        }

        private void PretendToPressKey(WebSocket socket, ControlRequest controlRequest)
        {
            Console.WriteLine("Pressed a key");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);

            await base.OnDisconnected(socket);

            Console.WriteLine($"{socketId} disconnected");
        }
    }
}
