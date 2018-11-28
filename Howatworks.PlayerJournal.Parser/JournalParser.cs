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

            try
            {
                var entry = (IJournalEntry) JsonConvert.DeserializeObject(line, mappedType, _serializerSettings);
                if (entry != null) return entry;
            }
            catch (JsonSerializationException e)
            {
                Trace.TraceError("JSON deserialisation failure {0}", e);
            }
            catch (FormatException e)
            {
                Trace.TraceError("Failure in type conversion {0}", e);
            }
            catch (Exception e)
            {
                Trace.TraceError("Unexpected exception parsing {0}", e);
            }
            Trace.TraceError("Failed to parse {0}", line);

            return null;

        }
    }
}
