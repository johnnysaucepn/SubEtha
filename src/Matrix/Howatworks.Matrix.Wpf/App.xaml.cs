using System.Windows;
using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using Howatworks.Thumb.Core;
using Howatworks.Matrix.Core;
using Howatworks.Thumb.Wpf;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;

namespace Howatworks.Matrix.Wpf
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
            var trayVm = _host.Services.GetRequiredService<TrayIconViewModel>();
            var authDialog = _host.Services.GetRequiredService<AuthenticationDialog>();
            LoadTaskbarIcon(trayVm, authDialog);
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
                    builder.AddThumbConfiguration("Matrix");
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
                    builder.RegisterModule(new MatrixModule());
                    builder.RegisterModule(new MatrixWpfModule());
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<MatrixBackgroundService>();
                });
        }

        private void LoadTaskbarIcon(TrayIconViewModel trayVm, AuthenticationDialog authDialog)
        {
            trayVm.OnExitApplication += (s, e) => Application.Current.Shutdown();
            trayVm.OnAuthenticationRequested += (s, e) => authDialog.Show();

            _tb.DataContext = trayVm;
            _tb?.BringIntoView();
        }
    }
}
