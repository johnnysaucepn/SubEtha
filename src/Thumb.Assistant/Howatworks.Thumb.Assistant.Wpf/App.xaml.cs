using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using Howatworks.Thumb.Assistant.Core;
using Howatworks.Thumb.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Howatworks.Thumb.Wpf;
using System.Threading;

namespace Howatworks.Thumb.Assistant.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _tb;
        private IContainer _container;
        private AssistantApp _app;
        private CancellationTokenSource _cts;

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
                _cts = new CancellationTokenSource();

                _app = _container.Resolve<AssistantApp>();
                _app.Run(_cts.Token);

                ViewManager.App = _app;
                _app.StartMonitoring();
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
    }
}
