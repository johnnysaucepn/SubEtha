using Microsoft.AspNetCore.Builder;

namespace Howatworks.Assistant.Core
{
    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder builder, WebSocketConnectionManager connectionManager)
        {
            return builder.UseMiddleware<WebSocketMiddleware>(connectionManager);
        }
    }
}
