using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketManager;
using WebSocketManager.Common;

namespace Thumb.Plugin.Controller.Handlers
{
    public class ControlHandler : WebSocketHandler
    {
        public ControlHandler(WebSocketConnectionManager webSocketConnectionManager)
            : base(webSocketConnectionManager, new ControllerMethodInvocationStrategy())
        {
            ((ControllerMethodInvocationStrategy) MethodInvocationStrategy).Controller = this;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);

            var message = new Message()
            {
                MessageType = MessageType.Text,
                Data = $"{socketId} is now connected"
            };

            await SendMessageToAllAsync(message);
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
                await SendMessageAsync(socket,
                    new Message()
                    {
                        MessageType = MessageType.Text, Data = message.ToUpperInvariant()
                    });
            }

        }

        // we ask a client to do some math for us then broadcast the results.
        private void PretendToPressKey(WebSocket socket, ControlRequest controlRequest)
        {
            Console.WriteLine("Pressed a key");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);

            await base.OnDisconnected(socket);

            var message = new Message()
            {
                MessageType = MessageType.Text,
                Data = $"{socketId} disconnected"
            };
            await SendMessageToAllAsync(message);
        }
    }
}
