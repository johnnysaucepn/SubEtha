using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Howatworks.Assistant.Core
{
    public enum AssistantMessageType
    {
        ActivateBinding,
        GetAvailableBindings,
        ControlState,
        AvailableBindings
    }

    public class AssistantMessage
    {
        public AssistantMessageType MessageType { get; }
        public JObject MessageContent { get; }

        public AssistantMessage(AssistantMessageType messageType, JObject messageContent)
        {
            MessageType = messageType;
            MessageContent = messageContent;
        }
    }

    public class WebSocketConnectionManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebSocketConnectionManager));

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            Converters = { new StringEnumConverter() }
        };

        private static readonly ConcurrentDictionary<Guid, WebSocket> WebSockets = new ConcurrentDictionary<Guid, WebSocket>();

        private readonly ISubject<AssistantMessage> _messagesReceived = new Subject<AssistantMessage>();
        public IObservable<AssistantMessage> MessagesReceived => _messagesReceived.AsObservable();

        private readonly ISubject<ClientConnectionChange> _connectionChanges = new Subject<ClientConnectionChange>();
        public IObservable<ClientConnectionChange> ConnectionChanges => _connectionChanges.AsObservable();

        public async Task Disconnect(Guid connectionId)
        {
            if (WebSockets.TryRemove(connectionId, out var socket))
            {
                try
                {
                    Log.Info($"Disconnecting '{connectionId}'...");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal disconnection", CancellationToken.None).ConfigureAwait(false);
                }
                catch (WebSocketException e)
                {
                    Log.Error($"Received error '{e.WebSocketErrorCode}', aborting socket");
                    socket.Abort();
                }

                _connectionChanges.OnNext(new ClientConnectionChange(ClientConnectionState.Disconnected, connectionId));
                Log.Info($"Disconnected '{connectionId}'");
            }
        }

        public async Task Connect(WebSocket socket)
        {
            Guid newConnectionId = Guid.NewGuid();
            if (!WebSockets.TryAdd(newConnectionId, socket))
            {
                Log.Error($"Could not add new websocket connection '{newConnectionId}', id already in use");
            }

            Log.Info($"Connected '{newConnectionId}'");
            _connectionChanges.OnNext(new ClientConnectionChange(ClientConnectionState.Connected, newConnectionId));

            while (socket.State == WebSocketState.Open)
            {
                var token = CancellationToken.None;
                var buffer = new ArraySegment<byte>(new byte[4096]);
                var received = await socket.ReceiveAsync(buffer, token).ConfigureAwait(false);

                Log.Debug($"Received a message of type {received.MessageType} from '{newConnectionId}'");

                switch (received.MessageType)
                {
                    case WebSocketMessageType.Close:
                        await Disconnect(newConnectionId).ConfigureAwait(false);
                        break;

                    case WebSocketMessageType.Text:
                        var incoming = Encoding.UTF8.GetString(buffer.Array ?? throw new InvalidOperationException(), buffer.Offset, buffer.Count);
                        try
                        {
                            Log.Debug($"Received message '{incoming}'");
                            var messageJObject = JObject.Parse(incoming);
                            var messageType = messageJObject["MessageType"].ToObject<AssistantMessageType>();
                            var messageContent = (JObject)messageJObject["MessageContent"];
                            var message = new AssistantMessage(messageType, messageContent);

                            _messagesReceived.OnNext(message);
                        }
                        catch (JsonException)
                        {
                            var errorMessage = Encoding.UTF8.GetBytes($"Failed to handle message: {incoming}");
                            await socket.SendAsync(new ArraySegment<byte>(errorMessage), WebSocketMessageType.Text, true, token).ConfigureAwait(false);
                        }
                        break;
                }
            }
            if (socket.State == WebSocketState.Aborted)
            {
                socket.Abort();
            }
        }

        public async Task DisconnectAll()
        {
            var attempts = 0;
            while (WebSockets.Count > 0 && attempts < 3)
            {
                foreach (var id in WebSockets.Keys)
                {
                    await Disconnect(id).ConfigureAwait(false);
                }
                attempts++;
            }
            if (attempts >= 3)
            {
                Log.Error("Failed to disconnect or abort all sockets");
            }
        }

        public async void SendMessageToAllClients(AssistantMessage message)
        {
            var messageString = JsonConvert.SerializeObject(message, _serializerSettings);
            var messageBytes = Encoding.UTF8.GetBytes(messageString);
            foreach (var id in WebSockets.Keys)
            {
                try
                {
                    if (WebSockets.TryGetValue(id, out var socket))
                    {
                        if (socket.State == WebSocketState.Open)
                        {
                            await socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);
                        }
                    }
                }
                catch (WebSocketException e)
                {
                    Log.Error(e);
                }
            }

            Log.Debug(message);
        }
    }
}