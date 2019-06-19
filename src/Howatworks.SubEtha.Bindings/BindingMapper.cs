using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Howatworks.SubEtha.Bindings
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

        public static BindingMapper FromFile(string bindingsPath)
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

        public IReadOnlyCollection<string> GetBoundButtons(params string[] devices)
        {
            var bindings =
                _buttonLookup.Value.Where(x =>
                    IsBound(devices, x));

            var bindingNames = bindings.Select(y => y.Key);

            return bindingNames.ToList();
        }

        private static bool IsBound(string[] devices, KeyValuePair<string, Button> buttonBinding)
        {
            if (buttonBinding.Value == null)
            {
                // Incorrectly-defined or missing bindings may return a null Button object when parsed
                return false;
            }
            return devices.Contains(buttonBinding.Value.Primary?.Device)
                   || devices.Contains(buttonBinding.Value.Secondary?.Device);
        }
    }
}
