using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Thumb.Plugin.Controller.Handlers;
using WebSocketManager;

namespace Thumb.Plugin.Controller
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddWebSocketManager(Assembly.GetExecutingAssembly());
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var staticContentAssembly = Assembly.GetExecutingAssembly();
            var staticContentFileProvider = new ManifestEmbeddedFileProvider(staticContentAssembly, "StaticContent");

            app

                .UseWebSockets()
                .MapWebSocketManager("/Controller", serviceProvider.GetService<ControlHandler>())
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = staticContentFileProvider
                })
                .UseMvcWithDefaultRoute();
        }
    }
}