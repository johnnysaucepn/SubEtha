using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Logging;
using Newtonsoft.Json;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalParser : IJournalParser
    {
        private static readonly ILog Log = LogManager.GetLogger<JournalParser>();

        private readonly Lazy<Dictionary<string, Type>> _eventTypeLookup = new Lazy<Dictionary<string, Type>>(
            () =>
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => typeof(JournalEntryBase).IsAssignableFrom(t))
                    .ToDictionary(
                        // Key is either the attribute value, if it exists, or otherwise the class name itself
                        t => t.GetTypeInfo().GetCustomAttributes<JournalNameAttribute>().FirstOrDefault()?.Name ?? t.Name,
                        t => t,
                        StringComparer.OrdinalIgnoreCase
                    );

            });

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public JournalEntryBase Parse(string gameVersion, string eventType, string line)
        {
            if (!_eventTypeLookup.Value.ContainsKey(eventType))
            {
                Log.Warn($"Found unrecognised journal entry type {eventType}: {line}");
                return null;
            }
            var mappedType = _eventTypeLookup.Value[eventType];

            JournalEntryBase entry = null;
            try
            {
                entry = JsonConvert.DeserializeObject(line, mappedType, _serializerSettings) as JournalEntryBase;
            }
            catch (JsonSerializationException e)
            {
                Log.Error($"Failed to parse {line}", e);
            }
            if (entry == null)
            {
                Log.Warn($"Failed to parse {line}");
                return null;
            }

            entry.GameVersionDiscriminator = gameVersion;
            return entry;
        }
    }
}
