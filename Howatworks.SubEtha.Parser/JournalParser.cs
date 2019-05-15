using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Howatworks.SubEtha.Journal;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Howatworks.SubEtha.Parser
{
    public class JournalParser : IJournalParser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalParser));

        private readonly Lazy<Dictionary<string, Type>> _entryTypeLookup = new Lazy<Dictionary<string, Type>>(
            () =>
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => typeof(IJournalEntry).IsAssignableFrom(t))
                    .ToDictionary(
                        // Key is either the attribute value, if it exists, or otherwise the class name itself
                        t => t.GetTypeInfo().GetCustomAttributes<JournalNameAttribute>().FirstOrDefault()?.Name ?? t.Name,
                        t => t,
                        StringComparer.OrdinalIgnoreCase
                    );

            });

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            // Enable strict reporting where event properties are provided that don't exist on the class
            // TODO: Remember to return this to MissingMemberHandling.Ignore when localisation check complete!
            MissingMemberHandling = MissingMemberHandling.Error,
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            MaxDepth = 7

        };

        /// <summary>
        /// Parse the absolute minimum required for an entry - saves time in deserialising
        /// to a specific type too early.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public (string, DateTimeOffset) ParseCommonProperties(string line)
        {
            //var json = JObject.Parse(line);
            var json = (JObject)JsonConvert.DeserializeObject(line, _serializerSettings);
            var eventType = json.Value<string>("event");
            var timeStamp = json.Value<DateTimeOffset>("timestamp");

            return (eventType, timeStamp);

        }

        public T Parse<T>(string line) where T : class
        {
            return Parse(typeof(T).Name, line) as T;
        }

        public IJournalEntry Parse(string eventType, string line)
        {
            if (!_entryTypeLookup.Value.ContainsKey(eventType))
            {
                throw new UnrecognizedJournalException(eventType, line);
            }
            var mappedType = _entryTypeLookup.Value[eventType];

            try
            {
                var entry = (IJournalEntry) JsonConvert.DeserializeObject(line, mappedType, _serializerSettings);
                if (entry != null) return entry;
            }
            catch (JsonSerializationException e)
            {
                throw new JournalParseException("JSON deserialisation failure", line, e);
            }
            catch (FormatException e)
            {
                throw new JournalParseException("Failure in type conversion", line, e);
            }
            catch (Exception e)
            {
                throw new JournalParseException("Unexpected exception parsing", line, e);
            }
            
            return null;

        }
    }
}
