using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using Howatworks.Thumb.Assistant.Core;
using Howatworks.Thumb.Core;
using System.Threading.Tasks;
using System.Windows;
using Howatworks.Thumb.Wpf;
using System.Threading;
using System;

namespace Howatworks.Thumb.Assistant.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _tb;
        private IContainer _container;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public void Start()
        {
            var config = new ThumbConfigBuilder("Assistant").Build();

            var logger = new Log4NetThumbLogging(config);
            logger.Configure();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbWpfModule(config));
            builder.RegisterModule(new AssistantModule());
            builder.RegisterModule(new AssistantWpfModule());

            _container = builder.Build();

            using (var scope = _container.BeginLifetimeScope())
            {
                var app = _container.Resolve<AssistantApp>();

                _tb = (TaskbarIcon)FindResource("ThumbTrayIcon");
                LoadTaskbarIcon();

                Task.Run(() =>
                {
                    var args = Environment.GetCommandLineArgs();
                    app.Run(args, _cts.Token);
                });
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Start();

            _tb = (TaskbarIcon)FindResource("ThumbTrayIcon");
            _tb?.BringIntoView();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _cts.Cancel();
            _tb.Visibility = Visibility.Hidden;
            _tb.Dispose();
        }

        public void LoadTaskbarIcon()
        {
            var trayVm = _container.Resolve<TrayIconViewModel>();

            trayVm.OnExitApplication += (s, e) => Application.Current.Shutdown();

            _tb.DataContext = trayVm;
            _tb?.BringIntoView();

        }
    }
}
