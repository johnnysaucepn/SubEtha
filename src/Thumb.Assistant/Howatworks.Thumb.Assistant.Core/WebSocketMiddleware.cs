using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Howatworks.Thumb.Assistant.Core
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
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();

                await _connectionManager.Connect(socket);


            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }


    }
}
