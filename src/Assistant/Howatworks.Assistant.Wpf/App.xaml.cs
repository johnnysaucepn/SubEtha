using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using Howatworks.Assistant.Core;
using Howatworks.Thumb.Core;
using System.Threading.Tasks;
using System.Windows;
using Howatworks.Thumb.Wpf;
using System.Threading;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Assistant.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _tb;
        private readonly IHost _host;

        public App()
        {
            var args = Environment.GetCommandLineArgs();
            _host = CreateHostBuilder(args)
                .Build();
        }

        public async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync().ConfigureAwait(true);

            _tb = (TaskbarIcon)FindResource("TrayIcon");
            LoadTaskbarIcon(_host.Services.GetRequiredService<TrayIconViewModel>());
        }

        public async void Application_Exit(object sender, ExitEventArgs e)
        {
            _tb.Visibility = Visibility.Hidden;
            _tb.Dispose();

            // Workaround for application shutdown hang: https://github.com/dotnet/extensions/issues/1363
            var lifetime = _host.Services.GetService<IHostLifetime>() as IDisposable;
            lifetime?.Dispose();

            await _host.StopAsync().ConfigureAwait(true);
            _host.Dispose();
        }

        [SuppressMessage("Simplification", "RCS1021:Convert lambda expression body to expression-body.", Justification = "Clarity and consistency")]
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddThumbConfiguration("Assistant");
                    builder.AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.UseThumbLogging(hostContext.Configuration);
                    logging.AddLog4Net();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((hostContext, builder) =>
                {
                    var config = hostContext.Configuration;
                    builder.RegisterModule(new ThumbCoreModule(config));
                    builder.RegisterModule(new ThumbWpfModule(config));
                    builder.RegisterModule(new AssistantModule());
                    builder.RegisterModule(new AssistantWpfModule());
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<AssistantBackgroundService>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        private void LoadTaskbarIcon(TrayIconViewModel trayVm)
        {
            trayVm.OnExitApplication += (s, e) => Application.Current.Shutdown();

            _tb.DataContext = trayVm;
            _tb?.BringIntoView();
        }
    }
}
