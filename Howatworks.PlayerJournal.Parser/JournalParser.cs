using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Newtonsoft.Json;

namespace Howatworks.PlayerJournal.Parser
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
            MaxDepth = 5

        };

        public IJournalEntry Parse(string eventType, string line)
        {
            if (!_entryTypeLookup.Value.ContainsKey(eventType))
            {
                Log.Warn($"Found unrecognised journal event type {eventType}: {line}");
                return null;
            }
            var mappedType = _entryTypeLookup.Value[eventType];

            try
            {
                var entry = (IJournalEntry) JsonConvert.DeserializeObject(line, mappedType, _serializerSettings);
                if (entry != null) return entry;
            }
            catch (JsonSerializationException e)
            {
                Log.Error($"JSON deserialisation failure: {e.Message}");
            }
            catch (FormatException e)
            {
                Log.Error("Failure in type conversion", e);
            }
            catch (Exception e)
            {
                Log.Error("Unexpected exception parsing", e);
            }
            Log.Error($"Failed to parse: {line}");

            return null;

        }
    }
}
