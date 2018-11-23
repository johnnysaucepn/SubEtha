using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Howatworks.PlayerJournal.Serialization;
using Newtonsoft.Json;

namespace Howatworks.PlayerJournal.Parser
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

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public IJournalEntry Parse(string eventType, string line)
        {
            if (!_entryTypeLookup.Value.ContainsKey(eventType))
            {
                Trace.TraceWarning($"Found unrecognised journal event type {eventType}: {line}");
                return null;
            }
            var mappedType = _entryTypeLookup.Value[eventType];

            IJournalEntry entry = null;
            try
            {
                entry = JsonConvert.DeserializeObject(line, mappedType, _serializerSettings) as IJournalEntry;
            }
            catch (JsonSerializationException e)
            {
                Trace.TraceError($"Failed to parse {line}", e);
            }

            if (entry != null) return entry;
            Trace.TraceWarning($"Failed to parse {line}");

            return null;

        }
    }
}
