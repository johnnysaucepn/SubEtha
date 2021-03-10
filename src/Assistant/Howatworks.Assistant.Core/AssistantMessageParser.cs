using Howatworks.Assistant.Core.Messages;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;

namespace Howatworks.Assistant.Core
{
    public class AssistantMessageParser
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(AssistantMessageParser));

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            Converters = { new StringEnumConverter() }
        };

        public IAssistantMessage Parse(string messageString)
        {
            var messageJObject = JObject.Parse(messageString);
            var messageTypeString = messageJObject["MessageType"].Value<string>(); // TODO: make more robust?
            var messageType = (AssistantMessageType)Enum.Parse(typeof(AssistantMessageType), messageTypeString);

            Log.Info($"Received '{messageType}' message '{messageString}'");

            return messageType switch
            {
                AssistantMessageType.ActivateBinding => JsonConvert.DeserializeObject<ActivateBindingMessage>(messageString, _serializerSettings),
                AssistantMessageType.StartActivateBinding => JsonConvert.DeserializeObject<StartActivateBindingMessage>(messageString, _serializerSettings),
                AssistantMessageType.EndActivateBinding => JsonConvert.DeserializeObject<EndActivateBindingMessage>(messageString, _serializerSettings),
                AssistantMessageType.GetAvailableBindings => JsonConvert.DeserializeObject<GetAvailableBindingsMessage>(messageString, _serializerSettings),
                _ => new RawMessage { MessageContent = messageString },
            };
        }
    }
}
