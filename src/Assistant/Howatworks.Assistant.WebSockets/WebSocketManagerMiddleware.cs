using log4net;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Howatworks.Assistant.WebSockets
{
    /// <summary>
    /// Adapted from code published at https://radu-matei.com/blog/aspnet-core-websockets-middleware
    /// </summary>
    public class WebSocketManagerMiddleware
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebSocketManagerMiddleware));

        private readonly RequestDelegate _next;
        private readonly WebSocketHandler _webSocketHandler;

        public WebSocketManagerMiddleware(RequestDelegate next,
                                            WebSocketHandler webSocketHandler)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                Log.Info("Connection is not WebSocket, pass through");
                await _next.Invoke(context).ConfigureAwait(false);
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
            await _webSocketHandler.OnConnected(socket).ConfigureAwait(false);

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await _webSocketHandler.ReceiveAsync(socket, result, buffer).ConfigureAwait(false);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocketHandler.OnDisconnected(socket).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                        cancellationToken: CancellationToken.None).ConfigureAwait(false);

                handleMessage(result, buffer);
            }
        }
    }
}
