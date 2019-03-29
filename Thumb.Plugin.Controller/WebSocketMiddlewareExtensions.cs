using Microsoft.AspNetCore.Builder;

namespace Thumb.Plugin.Controller
{
    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder builder, StatusManager statusManager)
        {
            return builder.UseMiddleware<WebSocketMiddleware>(statusManager);
        }
    }
}
