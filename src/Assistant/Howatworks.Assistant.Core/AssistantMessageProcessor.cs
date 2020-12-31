using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Howatworks.Assistant.Core.Messages;
using Howatworks.Assistant.WebSockets;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Howatworks.Assistant.Core
{
    public class AssistantMessageProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssistantMessageProcessor));

        private event Action<BindingActivationRequest> BindingActivated;
        public IObservable<BindingActivationRequest> BindingActivatedObservable;

        private event Action<string> BindingListRequested;
        public IObservable<string> BindingListRequestObservable;

        private readonly AssistantWebSocketHandler _handler;
        private readonly IDisposable _messageSub;

        public IObservable<string> NewConnection => _handler.NewConnection;

        public AssistantMessageProcessor(AssistantWebSocketHandler handler)
        {
            BindingActivatedObservable = Observable.FromEvent<BindingActivationRequest>(
                h => BindingActivated += h,
                h => BindingActivated -= h
            );

            BindingListRequestObservable = Observable.FromEvent<string>(
                h => BindingListRequested += h,
                h => BindingListRequested -= h
            );

            _handler = handler;

            _messageSub = _handler.MessageReceived.Subscribe(m =>
            {
                var messageType = (AssistantMessageType)Enum.Parse(typeof(AssistantMessageType), m.MessageType);

                Log.Info($"Received '{m.MessageType}' message '{m.MessageContent}'");
                switch (messageType)
                {
                    case AssistantMessageType.ActivateBinding:
                        var bindingActivationRequest = m.MessageContent.ToObject<BindingActivationRequest>();
                        BindingActivated?.Invoke(bindingActivationRequest);
                        break;
                    case AssistantMessageType.GetAvailableBindings:
                        BindingListRequested?.Invoke(m.SourceSocketId);
                        break;
                    default:
                        Log.Warn($"Unrecognised message type: {m.MessageType}");
                        break;
                }
            });
        }

        public async Task ReportAllBindings(string socketId, IReadOnlyCollection<string> bindingList)
        {
            var message = new OutgoingMessage(nameof(AssistantMessageType.AvailableBindings), JArray.FromObject(bindingList));
            await _handler.SendMessageAsync(socketId, JsonConvert.SerializeObject(message));
        }

        public async Task RefreshAllClients(ControlStateModel state)
        {
            var message = new OutgoingMessage(nameof(AssistantMessageType.ControlState), JObject.FromObject(state.CreateControlStateMessage()));
            await _handler.SendMessageToAllAsync(JsonConvert.SerializeObject(message)).ConfigureAwait(false);
        }

        public async Task RefreshClient(string socketId, ControlStateModel state)
        {
            var message = new OutgoingMessage(nameof(AssistantMessageType.ControlState), JObject.FromObject(state.CreateControlStateMessage()));
            await _handler.SendMessageAsync(socketId, JsonConvert.SerializeObject(message)).ConfigureAwait(false);
        }
    }
}