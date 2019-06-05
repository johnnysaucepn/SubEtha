using Microsoft.AspNetCore.Builder;

namespace Howatworks.Thumb.Plugin.Assistant
{
    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder builder, WebSocketConnectionManager connectionManager)
        {
            return builder.UseMiddleware<WebSocketMiddleware>(connectionManager);
        }
    }
}
