using System;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Thumb.Plugin.Controller
{
    public class ControllerJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ControllerJournalProcessorPlugin));

        private readonly IConfiguration _configuration;
        private readonly StatusManager _statusManager;
        public FlushBehaviour FlushBehaviour => FlushBehaviour.OnEveryBatch;
        public CatchupBehaviour FirstRunBehaviour => CatchupBehaviour.Skip;
        public CatchupBehaviour CatchupBehaviour => CatchupBehaviour.Skip;

        public event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        public event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;

        public ControllerJournalProcessorPlugin(IConfiguration configuration, IJournalMonitorNotifier notifier)
        {
            _configuration = configuration;
            _statusManager = new StatusManager(notifier);
        }

        public void Startup()
        {
            var hostBuilder = new WebHostBuilder()
                .UseConfiguration(_configuration)
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls("http://localhost:5984");

            var host = hostBuilder.Build();

            host.RunAsync().ConfigureAwait(false); // Don't block the calling thread
        }

        public void Apply(IJournalEntry journalEntry)
        {
            Log.Debug(JsonConvert.SerializeObject(journalEntry));

            if (_statusManager.Apply(journalEntry))
            {
                AppliedJournalEntries?.Invoke(this, new AppliedJournalEntriesEventArgs());
            }
        }

        public void Flush()
        {
            _statusManager.Flush();
            FlushedJournalProcessor?.Invoke(this, new FlushedJournalProcessorEventArgs());
        }
    }
}