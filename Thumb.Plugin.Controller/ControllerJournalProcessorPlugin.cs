using System;
using Autofac;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Thumb.Plugin.Controller
{
    public class ControllerJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ControllerJournalProcessorPlugin));

        private readonly IConfiguration _configuration;
        private readonly IComponentContext _context;
        private readonly StatusManager _statusManager;
        public FlushBehaviour FlushBehaviour => FlushBehaviour.OnEveryBatch;
        public CatchupBehaviour FirstRunBehaviour => CatchupBehaviour.Skip;
        public CatchupBehaviour CatchupBehaviour => CatchupBehaviour.Skip;

        public event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        public event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;

        public ControllerJournalProcessorPlugin(IConfiguration configuration, IComponentContext context, IJournalMonitorNotifier notifier, StatusManager statusManager)
        {
            _configuration = configuration;
            _context = context;
            _statusManager = statusManager;
        }

        public void Startup()
        {
            var hostBuilder = new WebHostBuilder()
                .UseConfiguration(_configuration)
                // Use our existing Autofac context in the web app services
                .ConfigureServices(services => { services.AddSingleton<IComponentContext>(_context); })
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