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
using Howatworks.Assistant.Core.Messages;
using Howatworks.Thumb.Core;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Howatworks.Assistant.Core
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

        public void Run(string[] args, CancellationToken token)
        {
            Log.Info("Starting up");

            _monitor.JournalFileWatch.Where(x => x.Action == JournalWatchAction.Started).Subscribe(
                e => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{e.File.FullName}'")
            );

            var bindingsPath = Path.Combine(_configuration["BindingsFolder"], _configuration["BindingsFilename"]);

            Log.Info($"Reading bindings from {bindingsPath}");
            _bindingMapper = BindingMapper.FromFile(bindingsPath);

            _connectionManager.MessagesReceived.Subscribe(m =>
            {
                Log.Info($"Received '{m.MessageType}' message '{m.MessageContent}'");
                switch (m.MessageType)
                {
                    case AssistantMessageType.ActivateBinding:
                        var controlRequest = m.MessageContent.ToObject<ControlRequest>();
                        ActivateBinding(controlRequest);
                        break;
                    case AssistantMessageType.GetAvailableBindings:
                        ReportAllBindings();
                        break;
                    default:
                        Log.Warn($"Unrecognised message type: {m.MessageType}");
                        break;
                }
            });

            // Every new connection gets the current state
            _connectionManager.ConnectionChanges.Where(c => c.NewState == ClientConnectionState.Connected).Subscribe(_ =>
            {
                RefreshAllClients(_statusManager.State);
            });

            _statusManager.ControlStateObservable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(c=>
            {
                _notifier.Notify(NotificationPriority.High, NotificationEventType.Update, "Updated control status");

                RefreshAllClients(c);
            });

            Log.Info("Creating app configuration and running ASP.NET pipeline");
            CreateHostBuilder(args).Build().RunAsync(token); // Don't block the calling thread

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
                    Log.Info($"Updated at {lastChecked}, last entry stamped {lastEntry}");
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

        private void ReportAllBindings()
        {
            var bindingList = _bindingMapper.GetBoundButtons("Keyboard", "Mouse");
            var message = new AssistantMessage(AssistantMessageType.AvailableBindings, JObject.FromObject(bindingList));
            /*var serializedMessage = JsonConvert.SerializeObject(new
            {
                MessageType = "AvailableBindings",
                MessageContent = bindingList
            },
                Formatting.Indented);*/
            _connectionManager.SendMessageToAllClients(message);
        }

        private void RefreshAllClients(ControlStateModel state)
        {
            var message = new AssistantMessage(AssistantMessageType.ControlState, JObject.FromObject(state.CreateControlStateMessage()));
            /*var serializedMessage = JsonConvert.SerializeObject(new
            {
                MessageType = "ControlState",
                MessageContent = state.CreateControlStateMessage()
            }, Formatting.Indented);*/
            _connectionManager.SendMessageToAllClients(message);
        }

        public IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .UseStartup<Startup>()
                .UseConfiguration(_configuration)
                .UseKestrel()
                .UseUrls("http://*:5984");
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton(_connectionManager);
            });

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