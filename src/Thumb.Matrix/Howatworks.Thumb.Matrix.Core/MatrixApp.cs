﻿using Howatworks.SubEtha.Monitor;
using Howatworks.Thumb.Core;
using System;
using Microsoft.Extensions.Configuration;
using log4net;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixApp : IThumbApp
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixApp));

        public LocationManager Location { get; }
        public ShipManager Ship { get; set; }
        public SessionManager Session { get; set; }

        public event EventHandler OnAuthenticationRequired;

        private readonly IConfiguration _config;
        private readonly JournalMonitorScheduler _monitor;
        private readonly IThumbNotifier _notifier;
        private readonly JournalEntryRouter _router;
        private readonly HttpUploadClient _client;

        public bool IsAuthenticated => _client.IsAuthenticated;
        public string SiteUri => _client.BaseUri.AbsoluteUri;

        private string _username;
        private string _password;

        // Empirically-determined to match the default ASP.NET settings
        public int MaxUsernameLength = 256;
        public int MaxPasswordLength = 100;

        public MatrixApp(
            IConfiguration config,
            JournalMonitorScheduler monitor,
            IThumbNotifier notifier,
            JournalEntryRouter router,
            LocationManager location,
            ShipManager ship,
            SessionManager session,
            HttpUploadClient client
        )
        {
            Location = location;
            Ship = ship;
            Session = session;
            _config = config;
            _monitor = monitor;
            _notifier = notifier;
            _router = router;
            _client = client;
        }

        public void Initialize()
        {
            Log.Info("Starting up");

            _monitor.JournalEntriesParsed += (sender, args) =>
            {
                if (args == null) return;
                try
                {
                    _router.Apply(args.Entries, args.BatchMode);
                }
                catch (MatrixAuthenticationException)
                {
                    StopMonitoring();
                    OnAuthenticationRequired?.Invoke(this, EventArgs.Empty);
                }
            };
            _monitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.Path}'");

            _monitor.JournalFileWatchingStopped += (sender, args) => _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{args.Path}'");

            _username = _config["Username"];
            _password = _config["Password"];

            // Try username and password from configuration, if possible
            var nowAuthenticated = Authenticate(_username, _password);
            // Otherwise, delegate getting username and password to caller
            // TODO: make this more structured
            if (!nowAuthenticated)
            {
                OnAuthenticationRequired?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Shutdown()
        {
            Log.Info("Shutting down");
            _monitor.Shutdown();
        }

        public bool Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(_username)) return false;
            if (string.IsNullOrWhiteSpace(_password)) return false;

            _client.AuthenticateByBearerToken(username, password);
            if (_client.IsAuthenticated)
            {
                StartMonitoring();
            }
            return _client.IsAuthenticated;
        }

        public void StartMonitoring()
        {
            Log.Info("Starting monitoring");
            _monitor.Start();
        }

        public void StopMonitoring()
        {
            Log.Info("Stopping monitoring");
            _monitor.Stop();
        }

        public DateTimeOffset? LastEntry()
        {
            return _monitor.LastEntry();
        }

        public DateTimeOffset? LastChecked()
        {
            return _monitor.LastChecked();
        }
    }
}