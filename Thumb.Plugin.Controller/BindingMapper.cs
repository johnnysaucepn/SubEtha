using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Howatworks.EliteDangerous.Bindings;

namespace Thumb.Plugin.Controller
{
    public class BindingMapper
    {
        private readonly BindingSet _bindingSet;

        private readonly Lazy<Dictionary<string, Button>> _buttonLookup;

        public BindingMapper(BindingSet bindingSet)
        {
            _bindingSet = bindingSet;

            _buttonLookup = new Lazy<Dictionary<string, Button>>(
                () =>
                {
                    return _bindingSet.GetType().GetProperties()
                        .Where(x => typeof(Button).IsAssignableFrom(x.PropertyType))
                        .ToDictionary(
                            // Key is either the attribute value, if it exists, or otherwise the class name itself
                            p => p.Name,
                            p => p.GetValue(_bindingSet) as Button,
                            StringComparer.OrdinalIgnoreCase
                        );

                });
        }

        public BindingMapper FromFile(string bindingsPath)
        {
            var serializer = new XmlSerializer(typeof(BindingSet), new XmlRootAttribute("Root"));
            using (var file = File.OpenRead(bindingsPath))
            {
                return new BindingMapper((BindingSet) serializer.Deserialize(file));
            }
        }

        public Button GetButtonBindingByName(string name)
        {
            return _buttonLookup.Value.ContainsKey(name) ? _buttonLookup.Value[name] : null;
        }
    }
}
