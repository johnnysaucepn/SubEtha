using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Howatworks.Configuration
{
    public class ConfigReader : IConfigReader
    {
        private readonly JToken _root;

        public ConfigReader(JToken root)
        {
            _root = root;
        }

        public T Get<T>(params string[] parts)
        {
            var node = TraverseConfiguration(parts);
            return node.Value<T>();
        }

        public IEnumerable<T> GetList<T>(params string[] parts)
        {
            var node = TraverseConfiguration(parts);
            return node.Values<T>();
        }

        private JToken TraverseConfiguration(string[] parts)
        {
            var node = _root;
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
