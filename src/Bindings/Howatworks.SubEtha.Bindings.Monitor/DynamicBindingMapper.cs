using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Howatworks.SubEtha.Bindings.Monitor
{
    /// <summary>
    /// An implementation of IBindingMapper that, instead of statically presenting a single binding set,
    /// monitors changes to .binds and config files to select the current set chosen by the player.
    /// <remarks>TODO: CustomFileWatcher doesn't appear to correctly track the file changes, therefore switching
    /// sets requires an app restart.</remarks>
    /// </summary>
    public class DynamicBindingMapper : IBindingMapper, IDisposable
    {
        //private static readonly SubEthaLog Log = SubEthaLog.GetLogger<DynamicBindingMapper>();

        private bool disposedValue;

        private readonly BindingMonitor _monitor;
        private SelectedPresets _activePresets = new SelectedPresets();
        private readonly Dictionary<string, BindingMapper> _bindingMappers = new Dictionary<string, BindingMapper>();

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly ISubject<Unit> _bindingsChanged = new Subject<Unit>();
        public IObservable<Unit> BindingsChanged => _bindingsChanged.AsObservable();

        public DynamicBindingMapper(BindingMonitor monitor)
        {
            _monitor = monitor;
            _disposables.Add(
                _monitor.BindingsChanged.Subscribe(bindingSet => Monitor_BindingsChanged(bindingSet))
                );

            _disposables.Add(
                _monitor.SelectedPresetChanged.Subscribe(presets => Monitor_SelectedPresetChanged(presets))
                );
        }

        private void Monitor_SelectedPresetChanged(SelectedPresets presets)
        {
            _activePresets = presets;

            /*ReplaceBindings(ref _general, presets.General);
            ReplaceBindings(ref _inShip, presets.InShip);
            ReplaceBindings(ref _driving, presets.Driving);
            ReplaceBindings(ref _onFoot, presets.OnFoot);*/

            _bindingsChanged.OnNext(Unit.Default);
        }

        /*private void ReplaceBindings(ref BindingMapper mapper, BindingSet bindingSet)
        {
            if (mapper?.GetPresetName().Equals(bindingSet.PresetName, StringComparison.InvariantCultureIgnoreCase) != true)
            {
                mapper = new BindingMapper(bindingSet);
            }
        }*/

        private void Monitor_BindingsChanged(BindingSet bindingSet)
        {
            var newMapper = new BindingMapper(bindingSet);
            var presetName = bindingSet.PresetName;

            _bindingMappers[presetName] = newMapper;

            /*foreach (var kvp in _bindingMappers)
            {
                if (kvp.Value.GetPresetName().Equals(bindingSet.PresetName))
                {
                    _bindingMappers[kvp.Key] = newMapper;
                }
            }*/

            _bindingsChanged.OnNext(Unit.Default);
        }


        public IReadOnlyCollection<BoundButton> GetBoundButtons(params string[] devices)
        {
            var comparer = new BoundButtonComparer();

            IEnumerable<BoundButton> collection = new List<BoundButton>();
            foreach (var mapper in _bindingMappers.Values.Distinct())
            {
                collection = collection.Union(mapper.GetBoundButtons(devices), comparer);
            }

            return collection.ToList();
            /*return _bindingMappers._general?.GetBoundButtons(devices)
                .Union(_inShip?.GetBoundButtons(devices), comparer)
                .Union(_driving?.GetBoundButtons(devices), comparer)
                .Union(_onFoot?.GetBoundButtons(devices), comparer)
                .ToList();*/
        }

        public Button GetButtonBindingByName(string name)
        {
            foreach (var mapper in _bindingMappers.Values.Distinct())
            {
                var mapped = mapper.GetButtonBindingByName(name);
                if (mapped != null) return mapped;
            }
            return null;
            /*
                return _general?.GetButtonBindingByName(name)
                ??
                _inShip?.GetButtonBindingByName(name)
                ??
                _driving?.GetButtonBindingByName(name)
                ??
                _onFoot?.GetButtonBindingByName(name);
            */
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _disposables.Dispose();
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
