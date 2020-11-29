using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Howatworks.Assistant.Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app)
        {
            var webSocketConnectionManager = app.ApplicationServices.GetService<WebSocketConnectionManager>();
            var staticContentAssembly = Assembly.GetExecutingAssembly();
            var staticContentFileProvider = new ManifestEmbeddedFileProvider(staticContentAssembly, "StaticContent");

            app
                .UseWebSockets()
                .UseWebSocketHandler(webSocketConnectionManager)
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = staticContentFileProvider
                })
                .UseMvc()
                .UseRouting();
        }
    }
}