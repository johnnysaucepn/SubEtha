using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Howatworks.Assistant.Core.Messages;
using Howatworks.Assistant.WebSockets;
using Howatworks.SubEtha.Bindings;
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

        private readonly GameControlBridge _keyboard;
        private readonly BindingMapper _bindingMapper;
        private readonly StatusManager _statusManager;
        private readonly AssistantMessageParser _messageParser;

        private readonly AssistantWebSocketHandler _handler;

        public AssistantMessageHub(
            AssistantWebSocketHandler handler,
            GameControlBridge keyboard,
            BindingMapper bindingMapper,
            StatusManager statusManager,
            AssistantMessageParser messageParser
            )
        {
            _handler = handler;
            _keyboard = keyboard;
            _bindingMapper = bindingMapper;
            _statusManager = statusManager;
            _messageParser = messageParser;

            _statusManager.ControlStateObservable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(async c => await RefreshAllClients(c).ConfigureAwait(false));

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
            });

            // Every new connection gets the current state
            _handler.NewConnection.Subscribe(async id => await RefreshClient(id, _statusManager.State).ConfigureAwait(false));
        }

        private async Task ReportAllBindings(string socketId)
        {
            var bindingList = _bindingMapper.GetBoundButtons("Keyboard", "Mouse");
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

            var button = _bindingMapper.GetButtonBindingByName(controlRequest.BindingName);
            if (button == null)
            {
                Log.Warn($"Unknown binding name found: '{controlRequest.BindingName}'");
            }
            else
            {
                _keyboard.ActivateKeyCombination(button);
            }
        }

        private void StartActivateBinding(StartActivateBindingMessage controlRequest)
        {
            Log.Info($"Started a control: '{controlRequest.BindingName}'");

            var button = _bindingMapper.GetButtonBindingByName(controlRequest.BindingName);
            if (button == null)
            {
                Log.Warn($"Unknown binding name found: '{controlRequest.BindingName}'");
            }
            else
            {
                _keyboard.HoldKeyCombination(button);
            }
        }

        private void EndActivateBinding(EndActivateBindingMessage controlRequest)
        {
            Log.Info($"Ended a control: '{controlRequest.BindingName}'");

            var button = _bindingMapper.GetButtonBindingByName(controlRequest.BindingName);
            if (button == null)
            {
                Log.Warn($"Unknown binding name found: '{controlRequest.BindingName}'");
            }
            else
            {
                _keyboard.ReleaseKeyCombination(button);
            }
        }
    }
}