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

            switch (messageType)
            {
                case AssistantMessageType.ActivateBinding:
                    return JsonConvert.DeserializeObject<ActivateBindingMessage>(messageString, _serializerSettings);
                case AssistantMessageType.GetAvailableBindings:
                    return JsonConvert.DeserializeObject<GetAvailableBindingsMessage>(messageString, _serializerSettings);
                default:
                    return new RawMessage { MessageContent = messageString };
            }
        }
    }
}
