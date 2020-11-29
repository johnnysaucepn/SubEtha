using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Howatworks.Assistant.Core
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketConnectionManager _connectionManager;

        public WebSocketMiddleware(RequestDelegate next, WebSocketConnectionManager connectionManager)
        {
            _next = next;
            _connectionManager = connectionManager;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);

                await _connectionManager.Connect(socket).ConfigureAwait(false);
            }
            else
            {
                await _next.Invoke(httpContext).ConfigureAwait(false);
            }
        }
    }
}
