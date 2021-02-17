using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howatworks.Assistant.Core.Messages;
using Howatworks.Assistant.WebSockets;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Howatworks.Assistant.Core
{
    public class AssistantMessageHub
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssistantMessageHub));

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            Converters = { new StringEnumConverter() }
        };

        private readonly GameControlBridge _controlBridge;
        private readonly StatusManager _statusManager;
        private readonly AssistantMessageParser _messageParser;

        private readonly AssistantWebSocketHandler _handler;

        public AssistantMessageHub(
            AssistantWebSocketHandler handler,
            GameControlBridge controlBridge,
            StatusManager statusManager,
            AssistantMessageParser messageParser
            )
        {
            _handler = handler;
            _controlBridge = controlBridge;
            _statusManager = statusManager;
            _messageParser = messageParser;
        }

        public void StartListening(CancellationToken token)
        {
            _statusManager.ControlStateObservable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(async c => await RefreshAllClients(c).ConfigureAwait(false), token);

            _handler.MessageReceived.Subscribe(async m =>
            {
                var sourceSocketId = m.SourceSocketId;

                switch (_messageParser.Parse(m.Message))
                {
                    case ActivateBindingMessage binding:
                        ActivateBinding(binding);
                        break;
                    case StartActivateBindingMessage binding:
                        StartActivateBinding(binding);
                        break;
                    case EndActivateBindingMessage binding:
                        EndActivateBinding(binding);
                        break;
                    case GetAvailableBindingsMessage getBindings:
                        await ReportAllBindings(sourceSocketId).ConfigureAwait(false);
                        break;
                }
            }, token);

            // Every new connection gets the current state
            _handler.ConnectionChanges
                .Where(c => c.Change == ConnectionChange.Connected)
                .Subscribe(async c => await RefreshClient(c.SocketId, _statusManager.State).ConfigureAwait(false), token);

            _controlBridge.SelectedBindingsChanged
                .Subscribe(async _ => await ReportAllBindingsToAllClients().ConfigureAwait(false), token);
        }

        private async Task ReportAllBindingsToAllClients()
        {
            var bindingList = _controlBridge.GetAllBoundButtons();
            var message = new AvailableBindingsMessage(bindingList);
            await _handler.SendMessageToAllAsync(JsonConvert.SerializeObject(message, _serializerSettings)).ConfigureAwait(false);
        }

        private async Task ReportAllBindings(string socketId)
        {
            var bindingList = _controlBridge.GetAllBoundButtons();
            var message = new AvailableBindingsMessage(bindingList);
            await _handler.SendMessageAsync(socketId, JsonConvert.SerializeObject(message, _serializerSettings)).ConfigureAwait(false);
        }

        private async Task RefreshAllClients(ControlStateModel state)
        {
            var message = state.CreateControlStateMessage();
            await _handler.SendMessageToAllAsync(JsonConvert.SerializeObject(message, _serializerSettings)).ConfigureAwait(false);
        }

        private async Task RefreshClient(string socketId, ControlStateModel state)
        {
            var message = state.CreateControlStateMessage();
            await _handler.SendMessageAsync(socketId, JsonConvert.SerializeObject(message, _serializerSettings)).ConfigureAwait(false);
        }

        private void ActivateBinding(ActivateBindingMessage controlRequest)
        {
            Log.Info($"Activated a control: '{controlRequest.BindingName}'");

            _controlBridge.ActivateKeyCombination(controlRequest.BindingName);
        }

        private void StartActivateBinding(StartActivateBindingMessage controlRequest)
        {
            Log.Info($"Started a control: '{controlRequest.BindingName}'");

            _controlBridge.HoldKeyCombination(controlRequest.BindingName);
        }

        private void EndActivateBinding(EndActivateBindingMessage controlRequest)
        {
            Log.Info($"Ended a control: '{controlRequest.BindingName}'");

            _controlBridge.ReleaseKeyCombination(controlRequest.BindingName);
        }
    }
}