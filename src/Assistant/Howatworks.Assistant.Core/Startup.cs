﻿using System.Reflection;
using Autofac;
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
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<WebSocketModule>();
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