using System;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Thumb.Plugin.Controller
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
        }

        public void Configure(IApplicationBuilder app)
        {
            var staticContentAssembly = Assembly.GetExecutingAssembly();
            var staticContentFileProvider = new ManifestEmbeddedFileProvider(staticContentAssembly, "StaticContent");

            app

                .UseWebSockets(new WebSocketOptions())
                .Use(async (context, next) =>
                {
                    if (context.Request.Path == "/ws")
                    {
                        if (context.WebSockets.IsWebSocketRequest)
                        {
                            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                            await Echo(context, webSocket);
                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                        }
                    }
                    else
                    {
                        await next();
                    }

                })
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = staticContentFileProvider
                })
                .UseMvcWithDefaultRoute();


        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                // Modify the content before echoing it back, to prove something has happened
                var sendBuffer = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(buffer).ToUpperInvariant());
                await webSocket.SendAsync(new ArraySegment<byte>(sendBuffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}