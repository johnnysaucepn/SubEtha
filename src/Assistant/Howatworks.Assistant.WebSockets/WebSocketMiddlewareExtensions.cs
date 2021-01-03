using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Howatworks.Assistant.WebSockets
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

        // Currently unused, we're registering services explicitly for now
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();

            //TODO: see if this is the best thing for us
            foreach (var type in Assembly.GetExecutingAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }
            services.AddSingleton<AssistantWebSocketHandler>();

            return services;
        }
    }
}
