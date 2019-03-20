using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Thumb.Plugin.Controller
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentBag<WebSocket> WebSockets = new ConcurrentBag<WebSocket>();

        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();

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
                            var incoming = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
                            try
                            {
                                var structuredMessage = JsonConvert.DeserializeObject<ControlRequest>(incoming);
                                PretendToPressKey(socket, structuredMessage);

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
            else
            {
                await _next.Invoke(httpContext);
            }
        }

        private void PretendToPressKey(WebSocket socket, ControlRequest controlRequest)
        {
            Console.WriteLine($"Pressed a key: {controlRequest.BindingName}");
        }
    }
}
