using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalParser : IJournalParser
    {
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
                Trace.TraceWarning($"Found unrecognised journal entry type {eventType}: {line}");
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
                Trace.TraceError($"Failed to parse {line}", e);
            }
            if (entry == null)
            {
                Trace.TraceWarning($"Failed to parse {line}");
                return null;
            }

            entry.GameVersionDiscriminator = gameVersion;
            return entry;
        }
    }
}
