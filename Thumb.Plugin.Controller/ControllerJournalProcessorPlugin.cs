using System;
using Autofac;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Thumb.Plugin.Controller.Messages;

namespace Thumb.Plugin.Controller
{
    public class ControllerJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ControllerJournalProcessorPlugin));

        private readonly IConfiguration _configuration;
        private readonly IComponentContext _context;
        private readonly IJournalMonitorNotifier _notifier;
        private readonly WebSocketConnectionManager _connectionManager;
        private readonly StatusManager _statusManager;
        public FlushBehaviour FlushBehaviour => FlushBehaviour.OnEveryBatch;
        public CatchupBehaviour FirstRunBehaviour => CatchupBehaviour.Skip;
        public CatchupBehaviour CatchupBehaviour => CatchupBehaviour.Skip;

        //public event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        public event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;

        public ControllerJournalProcessorPlugin(IConfiguration configuration, IComponentContext context, IJournalMonitorNotifier notifier, WebSocketConnectionManager connectionManager, StatusManager statusManager)
        {
            _configuration = configuration;
            _context = context;
            _notifier = notifier;
            _connectionManager = connectionManager;
            _statusManager = statusManager;
        }

        public void Startup()
        {
            var hostBuilder = new WebHostBuilder()
                .UseConfiguration(_configuration)
                // Use our existing Autofac context in the web app services
                .ConfigureServices(services => { services.AddSingleton(_context); })
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls("http://localhost:5984");

            var host = hostBuilder.Build();

            _connectionManager.MessageReceived += (sender, args) =>
            {
                var structuredMessage = JsonConvert.DeserializeObject<ControlRequest>(args.Message);
                _statusManager.ActivateBinding(structuredMessage);
            };

            _statusManager.ControlStateChanged += (sender, args) =>
            {
                _notifier.UpdatedService(args.State);
                var serializedMessage = JsonConvert.SerializeObject(args.State.CreateControlStateMessage(), Formatting.Indented);
                _connectionManager.SendMessageToAllClients(serializedMessage);
            };

            host.RunAsync().ConfigureAwait(false); // Don't block the calling thread
        }

        public void Flush()
        {
            _statusManager.Flush();
            FlushedJournalProcessor?.Invoke(this, new FlushedJournalProcessorEventArgs());
        }
    }
}