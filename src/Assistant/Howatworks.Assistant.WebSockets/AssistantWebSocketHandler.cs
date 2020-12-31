using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Howatworks.Assistant.WebSockets
{
    public class AssistantWebSocketHandler : WebSocketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssistantWebSocketHandler));

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            Converters = { new StringEnumConverter() }
        };

        private readonly ISubject<IncomingMessage> _messageReceived = new Subject<IncomingMessage>();
        public IObservable<IncomingMessage> MessageReceived => _messageReceived.AsObservable();

        private readonly ISubject<string> _newConnection = new Subject<string>();
        public IObservable<string> NewConnection => _newConnection.AsObservable();

        public AssistantWebSocketHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);
            Log.Info($"Connected '{socketId}'");
            _newConnection.OnNext(socketId);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            await base.OnDisconnected(socket);
            Log.Info($"Disconnected '{socketId}'");
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            var rawString = Encoding.UTF8.GetString(buffer, 0, result.Count);

            Log.Info($"Received '{rawString}' from '{socketId}'");

            var messageJObject = JObject.Parse(rawString);
            var messageType = messageJObject["MessageType"].Value<string>(); // TODO: make more robust?
            var messageContent = messageJObject.SelectToken("MessageContent");
            var message = new IncomingMessage(socketId, messageType, messageContent);

            Log.Info($"Received '{message.MessageType}' message '{message.MessageContent}'");

            _messageReceived.OnNext(message);
        }
    }
}
