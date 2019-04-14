using System;
using System.IO;
using Autofac;
using Howatworks.SubEtha.Bindings;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Howatworks.Thumb.Plugin.Assistant.Messages;

namespace Howatworks.Thumb.Plugin.Assistant
{
    public class AssistantJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssistantJournalProcessorPlugin));

        private readonly IConfiguration _configuration;
        private readonly IComponentContext _context;
        private readonly IJournalMonitorNotifier _notifier;
        private readonly WebSocketConnectionManager _connectionManager;
        private readonly StatusManager _statusManager;
        private readonly GameControlBridge _keyboard;
        private BindingMapper _bindingMapper;
        public FlushBehaviour FlushBehaviour => FlushBehaviour.OnEveryBatch;
        public CatchupBehaviour FirstRunBehaviour => CatchupBehaviour.Skip;
        public CatchupBehaviour CatchupBehaviour => CatchupBehaviour.Skip;

        //public event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        public event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;

        public AssistantJournalProcessorPlugin(
            IConfiguration configuration,
            IComponentContext context,
            IJournalMonitorNotifier notifier,
            WebSocketConnectionManager connectionManager,
            StatusManager statusManager,
            GameControlBridge keyboard)
        {
            _configuration = configuration;
            _context = context;
            _notifier = notifier;
            _connectionManager = connectionManager;
            _statusManager = statusManager;
            _keyboard = keyboard;
        }

        public void Startup()
        {
            var bindingsPath = Path.Combine(_configuration["BindingsFolder"], _configuration["BindingsFilename"]);

            _bindingMapper = BindingMapper.FromFile(bindingsPath);

            var hostBuilder = new WebHostBuilder()
                .UseConfiguration(_configuration)
                // Use our existing Autofac context in the web app services
                .ConfigureServices(services => { services.AddSingleton(_context); })
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls("http://*:5984");

            var host = hostBuilder.Build();

            _connectionManager.MessageReceived += (sender, args) =>
            {
                var messageWrapper = JObject.Parse(args.Message);
                switch (messageWrapper["MessageType"].Value<string>())
                {
                    case "ActivateBinding":
                        var controlRequest = messageWrapper["MessageContent"].ToObject<ControlRequest>();
                        ActivateBinding(controlRequest);
                        break;
                    default:
                        Log.Warn($"Unrecognised message format :{args.Message}");
                        break;
                }

            };

            _statusManager.ControlStateChanged += (sender, args) =>
            {
                _notifier.UpdatedService(args.State);
                var serializedMessage = JsonConvert.SerializeObject(new
                    {
                        MessageType = "ControlState", MessageContent = args.State.CreateControlStateMessage()
                    },
                    Formatting.Indented);
                _connectionManager.SendMessageToAllClients(serializedMessage);
            };

            host.RunAsync().ConfigureAwait(false); // Don't block the calling thread
        }

        public void Flush()
        {
            _statusManager.Flush();
            FlushedJournalProcessor?.Invoke(this, new FlushedJournalProcessorEventArgs());
        }

        private void ActivateBinding(ControlRequest controlRequest)
        {
            Log.Info($"Activated a control: {controlRequest.BindingName}");

            var button = _bindingMapper.GetButtonBindingByName(controlRequest.BindingName);
            if (button == null)
            {
                Log.Warn($"Unknown binding name found: '{controlRequest.BindingName}'");
            }

            _keyboard.TriggerKeyCombination(button);
        }
    }
}