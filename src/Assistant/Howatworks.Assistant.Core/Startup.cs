using System.Reflection;
using Howatworks.Assistant.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Howatworks.Assistant.Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            //services.AddWebSocketManager();
        }

        public void Configure(IApplicationBuilder app)
        {
            var webSocketHandler = app.ApplicationServices.GetService<WebSocketHandler>();

            var staticContentAssembly = Assembly.GetExecutingAssembly();
            var staticContentFileProvider = new ManifestEmbeddedFileProvider(staticContentAssembly, "StaticContent");

            app
                .UseWebSockets()
                .MapWebSocketManager("/Assistant", webSocketHandler)
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = staticContentFileProvider
                })
                .UseMvc()
                .UseRouting();
        }
    }
}