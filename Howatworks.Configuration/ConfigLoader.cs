using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Howatworks.Configuration
{
    public class ConfigLoader : IConfigLoader
    {
        private readonly JObject _jObject;

        public ConfigLoader(string filename)
        {
            var configJson = File.ReadAllText(filename);
            _jObject = JObject.Parse(configJson);
        }

        public IConfigReader GetConfigurationSection(params string[] parts)
        {
            var node = TraverseConfiguration(parts);
            return new ConfigReader(node);
        }

        private JToken TraverseConfiguration(string[] parts)
        {
            var node = _jObject.Root;
            foreach (var part in parts)
            {
                if (node[part] == null)
                {
                    throw new KeyNotFoundException("Could not find configuration key " + string.Join(",", parts));
                }
                node = node[part];
            }
            return node;
        }
    }
}
