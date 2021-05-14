using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Howatworks.SubEtha.Journal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Howatworks.SubEtha.Parser
{
    public class JournalParser : IJournalParser
    {
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

        public static IList<JsonConverter> AllConverters = new List<JsonConverter> { new ParentItemConverter() };

        private readonly JsonSerializerSettings _serializerSettings;

        private static readonly JsonSerializerSettings Strict = new JsonSerializerSettings
        {
            // Enable strict reporting where event properties are provided that don't exist on the class
            // TODO: Remember to return this to MissingMemberHandling.Ignore when localisation check complete!
            MissingMemberHandling = MissingMemberHandling.Error,
            NullValueHandling = NullValueHandling.Include,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            MaxDepth = 7,
            Converters = AllConverters
        };

        private static readonly JsonSerializerSettings Loose = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            MaxDepth = 7,
            Converters = AllConverters
        };

        public JournalParser(bool strict = false)
        {
            _serializerSettings = strict ? Strict : Loose;
        }

        /// <summary>
        /// Parse the absolute minimum required for an entry - saves time in deserialising
        /// to a specific type too early.
        /// </summary>
        /// <param name="line"></param>
        public (string eventType, DateTimeOffset timestamp) ParseCommonProperties(string line)
        {
            try
            {
                var json = (JObject)JsonConvert.DeserializeObject(line, _serializerSettings);
                string eventType = json.Value<string>("event");
                DateTimeOffset timestamp = json.Value<DateTimeOffset>("timestamp");

                return (eventType, timestamp);
            }
            catch (JsonSerializationException e)
            {
                throw new JournalParseException($"JSON deserialisation failure - {e.Message}", line, e);
            }
            catch (FormatException e)
            {
                throw new JournalParseException($"Failure in type conversion - {e.Message}", line, e);
            }
            catch (Exception e)
            {
                throw new JournalParseException($"Unexpected exception when parsing - {e.Message}", line, e);
            }
        }

        public T Parse<T>(string line) where T : class
        {
            return Parse(typeof(T).Name, line) as T;
        }

        public IJournalEntry Parse(string line)
        {
            var (entryType, _) = ParseCommonProperties(line);

            return Parse(entryType, line);
        }

        public IJournalEntry Parse(string entryType, string line)
        {
            // Quick check that this is an entry type that we are able to parse
            if (!_entryTypeLookup.Value.ContainsKey(entryType))
            {
                throw new UnrecognizedJournalException(entryType, line);
            }

            // What type of object will we serialize this to?
            var mappedType = _entryTypeLookup.Value[entryType];

            // Try to deserialize to the identified type, wrap any parsing exceptions
            try
            {
                var entry = (IJournalEntry) JsonConvert.DeserializeObject(line, mappedType, _serializerSettings);
                if (entry != null) return entry;
            }
            catch (JsonSerializationException e)
            {
                throw new JournalParseException($"JSON deserialisation failure - {e.Message}", line, e);
            }
            catch (FormatException e)
            {
                throw new JournalParseException($"Failure in type conversion - {e.Message}", line, e);
            }
            catch (Exception e)
            {
                throw new JournalParseException($"Unexpected exception when parsing - {e.Message}", line, e);
            }

            throw new JournalParseException("Parsing succeeded but data was empty", line);
        }
    }
}
