using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;

namespace Howatworks.Assistant.Core
{
    public class WebSocketMiddleware
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebSocketMiddleware));

        private readonly RequestDelegate _next;
        private readonly WebSocketConnectionManager _connectionManager;

        public WebSocketMiddleware(RequestDelegate next, WebSocketConnectionManager connectionManager)
        {
            _next = next;
            _connectionManager = connectionManager;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            Log.Info($"Checking incoming connection for {httpContext.Request}");
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                Log.Info("Connection is WebSocket");
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);

                await _connectionManager.Connect(socket).ConfigureAwait(false);
            }
            else
            {
                Log.Info("Connection is not WebSocket, pass through");
                await _next.Invoke(httpContext).ConfigureAwait(false);
            }
        }
    }
}
