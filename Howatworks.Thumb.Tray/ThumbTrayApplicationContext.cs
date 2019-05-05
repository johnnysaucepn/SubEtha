using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Autofac;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAPICodePack.Shell;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Tray.Properties;

namespace Howatworks.Thumb.Tray
{
    internal class ThumbTrayApplicationContext : ApplicationContext
    {
        private NotifyIcon _trayIcon;
        private ThumbApp _thumbApp;

        private static readonly ILog Log = LogManager.GetLogger(typeof(ThumbTrayApplicationContext));
        private IProgress<DateTimeOffset?> _progressHandler;

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
                ContextMenu = new ContextMenu(new[] {exitMenuItem}),
                Visible = true,
                Text = Resources.NotifyIconDefaultLabel
            };

            _progressHandler = new Progress<DateTimeOffset?>(_ =>
            {
                var lastChecked = _thumbApp.LastChecked().GetValueOrDefault(DateTimeOffset.UtcNow);
                var lastEntry = _thumbApp.LastEntry();
                _trayIcon.Text = lastEntry.HasValue
                    ? string.Format(
                        Resources.NotifyIconLastUpdatedLabel.Replace("\\n", Environment.NewLine),
                        lastEntry.Value.LocalDateTime.ToString("g"),
                        lastChecked.LocalDateTime.ToString("g"))
                    : Resources.NotifyIconNeverUpdatedLabel;
            });

            // TODO: In tray, we can use the Win32 package that exposes KnownFolders to retrieve SavedGames directly
            var defaultJournalFolder = Path.Combine(KnownFolders.SavedGames.Path, "Frontier Developments", "Elite Dangerous");

            var defaultBindingsFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Frontier Developments", "Elite Dangerous", "Options", "Bindings"
            );

            var defaultConfig = new Dictionary<string, string>
            {
                ["JournalFolder"] = defaultJournalFolder,
                ["JournalPattern"] = "Journal.*.log",
                ["RealTimeFilenames"] = "Status.json;Market.json;Outfitting.json;Shipyard.json",
                ["UpdateInterval"] = new TimeSpan(0, 0, 5).ToString(),
                ["BindingsFolder"] = defaultBindingsFolder,
                ["BindingsFilename"] = "Custom.3.0.binds",
                ["ActiveWindowTitle"] = "Elite - Dangerous (CLIENT)"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaultConfig)
                .AddJsonFile("config.json")
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
            _progressHandler.Report(_thumbApp.LastEntry());
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