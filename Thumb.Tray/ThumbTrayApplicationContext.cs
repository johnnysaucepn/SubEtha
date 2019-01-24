using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Autofac;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAPICodePack.Shell;
using Thumb.Core;
using Thumb.Tray.Properties;

namespace Thumb.Tray
{
    internal class ThumbTrayApplicationContext : ApplicationContext
    {
        private NotifyIcon _trayIcon;
        private ThumbApp _thumbApp;

        private static readonly ILog Log = LogManager.GetLogger(typeof(ThumbTrayApplicationContext));
        private IProgress<DateTime?> _progressHandler;

        private System.Threading.Timer _updateTimer;

        public ThumbTrayApplicationContext()
        {
            Application.ApplicationExit += Exit;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var exitMenuItem = new MenuItem(Resources.ExitLabel, (sender, args) => { Application.Exit(); });

            // Initialize Tray Icon
            _trayIcon = new NotifyIcon
            {
                Icon = Resources.ThumbIcon,
                ContextMenu = new ContextMenu(new[]
                {
                    exitMenuItem
                }),
                Visible = true,
                Text = Resources.NotifyIconDefaultLabel
            };
            _trayIcon.Text = Resources.NotifyIconNeverUpdatedLabel;

            _progressHandler = new Progress<DateTime?>(lastUpdated =>
            {
                _trayIcon.Text = lastUpdated.HasValue
                    ? string.Format(Resources.NotifyIconLastUpdatedLabel, lastUpdated.Value.ToShortDateString(),
                        lastUpdated.Value.ToShortTimeString())
                    : Resources.NotifyIconNeverUpdatedLabel;
            });

            // TODO: In tray, we can use the Win32 package that exposes KnownFolders to retrieve SavedGames directly
            var defaultJournalFolder = Path.Combine(KnownFolders.SavedGames.Path, "Frontier Developments", "Elite Dangerous");

            var defaultConfig = new Dictionary<string, string>
            {
                ["JournalFolder"] = defaultJournalFolder,
                ["JournalPattern"] = "Journal.*.log",
                ["RealTimeFilenames"] = "Status.json;Market.json;Outfitting.json;Shipyard.json",
                ["UpdateInterval"] = new TimeSpan(0, 0, 5).ToString()
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaultConfig)
                .AddJsonFile("journalmonitor.json")
                .Build();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbTrayModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                try
                {
                    _thumbApp = scope.Resolve<ThumbApp>();
                    _progressHandler.Report(null);

                    _updateTimer = new System.Threading.Timer(UpdateProgress, null, 0, 10000);

                    _thumbApp?.Start();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    Application.Exit();
                }
            }


        }

        private void UpdateProgress(object state)
        {
            _progressHandler.Report(_thumbApp.LastUpdated());
        }

        private void Exit(object sender, EventArgs e)
        {
            _thumbApp?.Stop();
            _updateTimer?.Dispose();
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
        }
    }
}