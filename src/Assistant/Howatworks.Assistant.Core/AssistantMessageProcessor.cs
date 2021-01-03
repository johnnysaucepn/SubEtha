using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Howatworks.Assistant.Core.Messages;
using Howatworks.Assistant.WebSockets;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Howatworks.Assistant.Core
{
    public class AssistantMessageProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssistantMessageProcessor));

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            Converters = { new StringEnumConverter() }
        };

        private event Action<ActivateBindingMessage> BindingActivated;
        public IObservable<ActivateBindingMessage> BindingActivatedObservable;

        private event Action<string> BindingListRequested;
        public IObservable<string> BindingListRequestObservable;

        private readonly AssistantWebSocketHandler _handler;

        public IObservable<string> NewConnection => _handler.NewConnection;

        public AssistantMessageProcessor(AssistantWebSocketHandler handler)
        {
            BindingActivatedObservable = Observable.FromEvent<ActivateBindingMessage>(
                h => BindingActivated += h,
                h => BindingActivated -= h
            );

            BindingListRequestObservable = Observable.FromEvent<string>(
                h => BindingListRequested += h,
                h => BindingListRequested -= h
            );

            _handler = handler;

            var parser = new AssistantMessageParser();

            var parsed = _handler.MessageReceived.Select(m => (sourceSocketId: m.SourceSocketId, message: parser.Parse(m.Message)));

            parsed.Where(m => m.message is ActivateBindingMessage).Subscribe(m => BindingActivated?.Invoke(m.message as ActivateBindingMessage));
            parsed.Where(m => m.message is GetAvailableBindingsMessage).Subscribe(m => BindingListRequested?.Invoke(m.sourceSocketId));
        }

        public async Task ReportAllBindings(string socketId, IReadOnlyCollection<string> bindingList)
        {
            var message = new AvailableBindingsMessage(bindingList);
            await _handler.SendMessageAsync(socketId, JsonConvert.SerializeObject(message, _serializerSettings)).ConfigureAwait(false);
        }

        public async Task RefreshAllClients(ControlStateModel state)
        {
            var message = state.CreateControlStateMessage();
            await _handler.SendMessageToAllAsync(JsonConvert.SerializeObject(message, _serializerSettings)).ConfigureAwait(false);
        }

        public async Task RefreshClient(string socketId, ControlStateModel state)
        {
            var message = state.CreateControlStateMessage();
            await _handler.SendMessageAsync(socketId, JsonConvert.SerializeObject(message, _serializerSettings)).ConfigureAwait(false);
        }
    }
}