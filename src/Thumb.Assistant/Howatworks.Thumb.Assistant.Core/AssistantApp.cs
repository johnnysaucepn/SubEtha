using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Howatworks.SubEtha.Bindings;
using Howatworks.SubEtha.Monitor;
using Howatworks.SubEtha.Parser;
using Howatworks.Thumb.Assistant.Core.Messages;
using Howatworks.Thumb.Core;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Howatworks.Thumb.Assistant.Core
{
    public class AssistantApp
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssistantApp));

        private readonly IConfiguration _configuration;
        private readonly IJournalMonitorState _state;

        private readonly LiveJournalMonitor _monitor;
        private readonly IThumbNotifier _notifier;
        private readonly IJournalParser _parser;

        private readonly WebSocketConnectionManager _connectionManager;
        private readonly StatusManager _statusManager;
        private readonly GameControlBridge _keyboard;
        private BindingMapper _bindingMapper;

        private readonly Subject<DateTimeOffset> _updateSubject = new Subject<DateTimeOffset>();
        public IObservable<Timestamped<DateTimeOffset>> Updates => _updateSubject
                                                                    .Throttle(TimeSpan.FromSeconds(5))
                                                                    .Timestamp()
                                                                    .AsObservable();

        public AssistantApp(
            IConfiguration configuration,
            IJournalMonitorState state,
            LiveJournalMonitor monitor,
            IThumbNotifier notifier,
            IJournalParser parser,
            WebSocketConnectionManager connectionManager,
            StatusManager statusManager,
            GameControlBridge keyboard)
        {
            _configuration = configuration;
            _state = state;

            _monitor = monitor;
            _notifier = notifier;
            _parser = parser;

            _connectionManager = connectionManager;
            _statusManager = statusManager;
            _keyboard = keyboard;
        }

        public void Run(CancellationToken token)
        {
            Log.Info("Starting up");

            _monitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.File.FullName}'");

            var bindingsPath = Path.Combine(_configuration["BindingsFolder"], _configuration["BindingsFilename"]);

            _bindingMapper = BindingMapper.FromFile(bindingsPath);

            _connectionManager.MessageReceived += (_, args) =>
            {
                var messageWrapper = JObject.Parse(args.Message);
                switch (messageWrapper["MessageType"].Value<string>())
                {
                    case "ActivateBinding":
                        var controlRequest = messageWrapper["MessageContent"].ToObject<ControlRequest>();
                        ActivateBinding(controlRequest);
                        break;
                    case "GetAvailableBindings":
                        var bindingList = _bindingMapper.GetBoundButtons("Keyboard", "Mouse");
                        var serializedMessage = JsonConvert.SerializeObject(new
                            {
                                MessageType = "AvailableBindings",
                                MessageContent = bindingList
                            },
                            Formatting.Indented);
                        _connectionManager.SendMessageToAllClients(serializedMessage);
                        break;
                    default:
                        Log.Warn($"Unrecognised message format: {args.Message}");
                        break;
                }
            };

            _connectionManager.ClientConnected += (sender, args) =>
            {
                var serializedMessage = JsonConvert.SerializeObject(new
                    {
                        MessageType = "ControlState",
                        MessageContent = _statusManager.CreateControlStateMessage()
                },
                    Formatting.Indented);
                _connectionManager.SendMessageToAllClients(serializedMessage);
            };

            _statusManager.ControlStateObservable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(c=>
            {
                _notifier.Notify(NotificationPriority.High, NotificationEventType.Update, "Updated control status");
                var serializedMessage = JsonConvert.SerializeObject(new
                    {
                        MessageType = "ControlState", MessageContent = c.CreateControlStateMessage()
                    },
                    Formatting.Indented);
                _connectionManager.SendMessageToAllClients(serializedMessage);
            });

            var hostBuilder = new WebHostBuilder()
                .UseConfiguration(_configuration)
                // Use our existing Autofac context in the web app services
                .ConfigureServices(services => services.AddSingleton(_connectionManager))
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls("http://*:5984");

            var host = hostBuilder.Build();

            host.RunAsync(token).ConfigureAwait(false); // Don't block the calling thread

            var startTime = _state.LastEntrySeen ?? DateTimeOffset.MinValue;
            var source = new JournalEntrySource(_parser, startTime, _monitor);

            var publisher = new JournalEntryPublisher(source);
            var publication = publisher.Observable.Publish();

            _statusManager.SubscribeTo(publication);

            publication.Subscribe(t =>
            {
                _updateSubject.OnNext(t.Entry.Timestamp);
            });

            Updates
                .Subscribe(e =>
                {
                    var lastEntry = e.Value;
                    var lastChecked = e.Timestamp;
                    _state.Update(lastChecked, lastEntry);
                });

            using (publication.Connect())
            {
                while (!token.IsCancellationRequested)
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();

                    publisher.Poll();
                }
            }
        }

        public DateTimeOffset? LastEntry => _state.LastEntrySeen;

        public DateTimeOffset? LastChecked => _state.LastChecked;

        private void ActivateBinding(ControlRequest controlRequest)
        {
            Log.Info($"Activated a control: '{controlRequest.BindingName}'");

            var button = _bindingMapper.GetButtonBindingByName(controlRequest.BindingName);
            if (button == null)
            {
                Log.Warn($"Unknown binding name found: '{controlRequest.BindingName}'");
            }
            else
            {
                _keyboard.TriggerKeyCombination(button);
            }
        }
    }
}