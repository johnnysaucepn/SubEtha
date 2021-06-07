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
        private BindingMapper _general;
        private BindingMapper _inShip;
        private BindingMapper _driving;
        private BindingMapper _onFoot;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly ISubject<Unit> _bindingsChanged = new Subject<Unit>();
        public IObservable<Unit> BindingsChanged { get; }

        public DynamicBindingMapper(BindingMonitor monitor)
        {
            _monitor = monitor;
            _disposables.Add(
                _monitor.BindingsChanged.Subscribe(presetName => Monitor_BindingsChanged(presetName))
                );

            _disposables.Add(
                _monitor.SelectedPresetChanged.Subscribe(presets => Monitor_SelectedPresetChanged(presets))
                );
        }

        private void Monitor_SelectedPresetChanged(SelectedPresets presets)
        {
            ReplaceBindings(ref _general, presets.General);
            ReplaceBindings(ref _inShip, presets.InShip);
            ReplaceBindings(ref _driving, presets.Driving);
            ReplaceBindings(ref _onFoot, presets.OnFoot);

            _bindingsChanged.OnNext(Unit.Default);
        }

        private void ReplaceBindings(ref BindingMapper mapper, string newPreset)
        {
            if (!mapper.GetPresetName().Equals(newPreset, StringComparison.InvariantCultureIgnoreCase))
            {
                mapper = new BindingMapper(_monitor.GetBindingSet(newPreset));
            }
        }

        private void Monitor_BindingsChanged(string presetName)
        {
            var newBinding = new BindingMapper(_monitor.GetBindingSet(presetName));

            ReplaceBinding(ref _general, newBinding);
            ReplaceBinding(ref _inShip, newBinding);
            ReplaceBinding(ref _driving, newBinding);
            ReplaceBinding(ref _onFoot, newBinding);

            _bindingsChanged.OnNext(Unit.Default);
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
