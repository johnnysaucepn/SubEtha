using Howatworks.SubEtha.Common;
using Howatworks.SubEtha.Common.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Howatworks.SubEtha.Bindings.Monitor
{
    /// <summary>
    /// An implementation of IBindingMapper that, instead of statically presenting a single binding set,
    /// monitors changes to .binds and config files to select the current set chosen by the player.
    /// <remarks>TODO: CustomFileWatcher doesn't appear to correctly track the file changes, therefore switching
    /// sets requires an app restart.</remarks>
    /// </summary>
    public class DynamicBindingMapper : IDisposable
    {
        //private static readonly SubEthaLog Log = SubEthaLog.GetLogger<DynamicBindingMapper>();

        private bool disposedValue;

        private readonly BindingMonitor _monitor;
        private BindingMapper _general;
        private BindingMapper _inShip;
        private BindingMapper _driving;
        private BindingMapper _onFoot;

        public DynamicBindingMapper(BindingMonitor monitor)
        {
            _monitor = monitor;
            _monitor.BindingsChanged += Monitor_BindingsChanged;
            _monitor.SelectedPresetChanged += Monitor_SelectedPresetChanged;
        }

        private void Monitor_SelectedPresetChanged(object sender, SelectedPresetChangedEventArgs e)
        {
            ReplaceBindings(ref _general, e.NewSelectedPresets.General);
            ReplaceBindings(ref _inShip, e.NewSelectedPresets.InShip);
            ReplaceBindings(ref _driving, e.NewSelectedPresets.Driving);
            ReplaceBindings(ref _onFoot, e.NewSelectedPresets.OnFoot);
        }

        private void ReplaceBindings(ref BindingMapper mapper, string newPreset)
        {
            if (!mapper.GetPresetName().Equals(newPreset, StringComparison.InvariantCultureIgnoreCase))
            {
                mapper = new BindingMapper(_monitor.GetBindingSet(newPreset));
            }
        }

        private void Monitor_BindingsChanged(object sender, BindingsChangedEventArgs e)
        {
            var newBinding = new BindingMapper(_monitor.GetBindingSet(e.PresetName));

            ReplaceBinding(ref _general, newBinding);
            ReplaceBinding(ref _inShip, newBinding);
            ReplaceBinding(ref _driving, newBinding);
            ReplaceBinding(ref _onFoot, newBinding);
        }

        private void ReplaceBinding(ref BindingMapper oldMapper, BindingMapper newMapper)
        {
            if (oldMapper.GetPresetName().Equals(newMapper.GetPresetName()))
            {
                oldMapper = newMapper;
            }
        }

        public IReadOnlyCollection<BoundButton> GetBoundButtons(params string[] devices)
        {
            var comparer = new BoundButtonComparer();
            return _general?.GetBoundButtons(devices)
                .Union(_inShip?.GetBoundButtons(devices), comparer)
                .Union(_driving?.GetBoundButtons(devices), comparer)
                .Union(_onFoot?.GetBoundButtons(devices), comparer)
                .ToList();
        }

        public Button GetButtonBindingByName(string name)
        {
            return _general?.GetButtonBindingByName(name)
                ??
                _inShip?.GetButtonBindingByName(name)
                ??
                _driving?.GetButtonBindingByName(name)
                ??
                _onFoot?.GetButtonBindingByName(name);
        }

        /*private BindingMapper GetCurrentBindingMapper()
        {
            if (_currentSelectedPresetName == null)
            {
                return null;
            }
            if (_allBindingsByPresetName.TryGetValue(_currentSelectedPresetName, out var selectedBindingMapper))
            {
                return selectedBindingMapper;
            }
            Log.Warn("No binding preset selected");
            return null;
        }*/

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _monitor.BindingsChanged -= Monitor_BindingsChanged;
                    _monitor.SelectedPresetChanged -= Monitor_SelectedPresetChanged;

                    //_allBindingSubscription?.Dispose();
                    //_selectedBindingSubscription?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
