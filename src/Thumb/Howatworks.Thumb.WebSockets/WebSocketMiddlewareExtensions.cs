using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Howatworks.Thumb.WebSockets
{
    /// <summary>
    /// Adapted from code published at https://radu-matei.com/blog/aspnet-core-websockets-middleware
    /// </summary>
    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                        PathString path,
                                                        WebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }
    }
}
