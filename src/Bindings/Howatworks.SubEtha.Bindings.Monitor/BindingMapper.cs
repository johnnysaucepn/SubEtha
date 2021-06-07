﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Howatworks.SubEtha.Bindings.Monitor
{
    public class BindingMapper : IBindingMapper
    {
        private readonly BindingSet _bindingSet;

        private readonly Lazy<Dictionary<string, Button>> _buttonLookup;

        public IObservable<Unit> BindingsChanged => Observable.Never<Unit>(); //not currently used - static bindings don't change

        public BindingMapper(BindingSet bindingSet)
        {
            _bindingSet = bindingSet;

            _buttonLookup = new Lazy<Dictionary<string, Button>>(() =>
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

        public string GetPresetName()
        {
            return _bindingSet.PresetName;
        }

        public Button GetButtonBindingByName(string name)
        {
            return _buttonLookup.Value.ContainsKey(name) ? _buttonLookup.Value[name] : null;
        }

        public IReadOnlyCollection<BoundButton> GetBoundButtons(params string[] devices)
        {
            var bindings = _buttonLookup.Value.Where(x => IsBound(devices, x));

            var boundButtons = bindings.Select(y =>
            {
                var bindingName = y.Key;
                if (y.Value is ToggleButton t)
                {
                    var toggleMode = t.ToggleOn.Value ? BindingActivationType.Press : BindingActivationType.Hold;
                    return new BoundButton(bindingName, toggleMode);
                }
                return new BoundButton(bindingName);
            });

            return boundButtons.ToList();
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
