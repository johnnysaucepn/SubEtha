using Howatworks.SubEtha.Common;
using Howatworks.SubEtha.Common.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Howatworks.SubEtha.Bindings.Monitor
{
    public class BindingMonitor : IDisposable
    {
        private static readonly SubEthaLog Log = SubEthaLog.GetLogger<BindingMonitor>();

        private readonly CustomFileWatcher _allBindingFileWatcher;
        private readonly CustomFileWatcher _selectedBindingFileWatcher;

        private readonly IDisposable _allBindingSubscription;
        private readonly IDisposable _selectedBindingSubscription;

        private readonly Dictionary<string, BindingSet> _allBindingsByPresetName = new Dictionary<string, BindingSet>();

        private bool disposedValue;

        private readonly ISubject<string> _bindingsChanged = new Subject<string>();
        public IObservable<string> BindingsChanged => _bindingsChanged.AsObservable();

        private readonly ISubject<SelectedPresets> _selectedPresetChanged = new Subject<SelectedPresets>();
        public IObservable<SelectedPresets> SelectedPresetChanged => _selectedPresetChanged.AsObservable();

        public BindingMonitor(IConfiguration config)
        {
            var folder = config["BindingsFolder"];

            // Watch for any new binding files coming in or being changed
            _allBindingFileWatcher = new CustomFileWatcher(folder, "*.binds");

            // When they do, make sure we've read and parsed the file and add it to our list.
            // This lets us switch as soon as the value in StartPreset.start changes, without
            // having to scan all the files again to find the one with that name.
            _allBindingSubscription = _allBindingFileWatcher.CreatedFiles
                .Merge(_allBindingFileWatcher.ChangedFiles)
                .Subscribe(x =>
                {
                    // Load the .binds file
                    var path = new FileInfo(Path.Combine(folder, x));
                    var updatedBinding = new BindingSetReader(path).Read();
                    var presetName = updatedBinding.PresetName;
                    _allBindingsByPresetName[presetName] = updatedBinding;

                    _bindingsChanged.OnNext(presetName);
                });

            // Now start raising file events
            _allBindingFileWatcher.Start();

            // Watch for a new preset being selected
            _selectedBindingFileWatcher = new CustomFileWatcher(folder, "StartPreset.start");

            // When it does, switch to that binding set, and notify clients
            _selectedBindingSubscription = _selectedBindingFileWatcher.CreatedFiles
                .Merge(_selectedBindingFileWatcher.ChangedFiles)
                // FileWatcher can still produce multiple notifications for the same thing. For that matter,
                // users may flick through a few selections before setting. Throttle the updates.
                .Throttle(TimeSpan.FromSeconds(3))
                .Subscribe(x =>
                {
                    Log.Info("Checking for new binding presets");
                    var currentSelectedPresetNames = ReadStartPresets(new FileInfo(Path.Combine(folder, x)));
                    Log.Info($"Selecting binding presets {currentSelectedPresetNames}");
                    // Refresh everyone's list of available bindings
                    _selectedPresetChanged.OnNext(currentSelectedPresetNames);
                });

            // Now start raising file events
            _selectedBindingFileWatcher.Start();
        }

        public BindingSet GetBindingSet(string startPresetName)
        {
            // TODO: handle exceptions
            return _allBindingsByPresetName[startPresetName];
        }

        private SelectedPresets ReadStartPresets(FileInfo startPresetFile)
        {
            Log.Info($"Reading selected preset from '{startPresetFile.Name}'");
            try
            {
                var presetNameText = File.ReadAllText(startPresetFile.FullName);

                if (string.IsNullOrWhiteSpace(presetNameText))
                {
                    Log.Warn("No preset file content found");
                    return null;
                }

                // Split by \n and \r and remove any empty entries
                var presetNames = presetNameText
                    .Split(Environment.NewLine.ToArray())
                    .Where(x=> !string.IsNullOrWhiteSpace(x))
                    .ToList();
                // TODO: get the order of these right
                return new SelectedPresets
                {
                    General = presetNames.Count > 0 ? presetNames[0] : null,
                    InShip = presetNames.Count > 1 ? presetNames[1] : null,
                    Driving = presetNames.Count > 2 ? presetNames[2] : null,
                    OnFoot = presetNames.Count > 3 ? presetNames[3] : null
                };
            }
            catch (IOException e)
            {
                Log.Warn("Could not read preset", e);
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _allBindingSubscription?.Dispose();
                    _selectedBindingSubscription?.Dispose();
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
