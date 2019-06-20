using System;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Howatworks.Thumb.Plugin.Assistant
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var autofacContext = serviceProvider.GetService<IComponentContext>();
            var staticContentAssembly = Assembly.GetExecutingAssembly();
            var staticContentFileProvider = new ManifestEmbeddedFileProvider(staticContentAssembly, "StaticContent");

            var webSocketConnectionManager = autofacContext.Resolve<WebSocketConnectionManager>();

            app
                .UseWebSockets()
                .UseWebSocketHandler(webSocketConnectionManager)
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = staticContentFileProvider
                })
                .UseMvcWithDefaultRoute();
        }
    }
}