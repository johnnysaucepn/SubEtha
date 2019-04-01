using Microsoft.AspNetCore.Builder;

namespace Thumb.Plugin.Controller
{
    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder builder, WebSocketConnectionManager connectionManager)
        {
            return builder.UseMiddleware<WebSocketMiddleware>(connectionManager);
        }
    }
}
