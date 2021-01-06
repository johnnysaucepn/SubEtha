using Howatworks.SubEtha.Bindings;
using Howatworks.SubEtha.Monitor;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;

namespace Howatworks.Assistant.Core
{
    /// <summary>
    /// An implementation of IBindingMapper that, instead of statically presenting a single binding set,
    /// monitors changes to .binds and config files to select the current set chosen by the player.
    /// <remarks>TODO: CustomFileWatcher doesn't appear to correctly track the file changes, therefore switching
    /// sets requires an app restart.</remarks>
    /// </summary>
    public class DynamicBindingMapper : IBindingMapper, IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DynamicBindingMapper));

        private readonly Dictionary<string, BindingMapper> _allBindingsByPresetName = new Dictionary<string, BindingMapper>();

        private readonly CustomFileWatcher _allBindingFileWatcher;
        private readonly CustomFileWatcher _selectedBindingFileWatcher;
        private string _currentSelectedPresetName;
        private bool disposedValue;

        private readonly IDisposable _allBindingSubscription;
        private readonly IDisposable _selectedBindingSubscription;

        public event EventHandler BindingsChanged;

        public DynamicBindingMapper(IConfiguration config)
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
                    var updatedBinding = BindingMapper.FromFile(Path.Combine(folder, x));
                    var presetName = updatedBinding.GetPresetName();
                    _allBindingsByPresetName[presetName] = updatedBinding;

                    // If we've seen a change in the current selected preset, then refresh everyone
                    if (string.Equals(presetName, _currentSelectedPresetName))
                    {
                        BindingsChanged?.Invoke(this, EventArgs.Empty);
                    }
                });

            // Now start raising file events
            _allBindingFileWatcher.Start();

            // Watch for a new preset being selected
            _selectedBindingFileWatcher = new CustomFileWatcher(folder, "StartPreset.start");

            // When it does, switch to that binding set, and notify clients
            _selectedBindingSubscription = _selectedBindingFileWatcher.CreatedFiles
                .Merge(_selectedBindingFileWatcher.ChangedFiles)
                // FileWatcher can still produce multiple notifications for the same thing. For that matter,
                // users may flick through a few selection before setting. Throttle the updates.
                .Throttle(TimeSpan.FromSeconds(3))
                .Subscribe(x =>
                {
                    Log.Info("Checking for new binding preset");
                    _currentSelectedPresetName = ReadStartPreset(Path.Combine(folder, x));
                    Log.Info($"Selecting binding preset {_currentSelectedPresetName}");
                    // Refresh everyone's list of available bindings
                    BindingsChanged?.Invoke(this, EventArgs.Empty);
                });

            // Now start raising file events
            _selectedBindingFileWatcher.Start();
        }

        public IReadOnlyCollection<string> GetBoundButtons(params string[] devices)
        {
            return GetCurrentBindingMapper()?.GetBoundButtons(devices);
        }

        public Button GetButtonBindingByName(string name)
        {
            return GetCurrentBindingMapper()?.GetButtonBindingByName(name);
        }

        public string GetPresetName()
        {
            return GetCurrentBindingMapper()?.GetPresetName();
        }

        private BindingMapper GetCurrentBindingMapper()
        {
            if (_allBindingsByPresetName.TryGetValue(_currentSelectedPresetName, out var selectedBindingMapper))
            {
                return selectedBindingMapper;
            }
            Log.Warn("No binding preset selected");
            return null;
        }

        private string ReadStartPreset(string startPresetFilename)
        {
            Log.Info($"Reading selected preset from {startPresetFilename}");
            string presetName;
            try
            {
                presetName = File.ReadAllText(startPresetFilename);
            }
            catch (IOException e)
            {
                Log.Warn("Could not read preset", e);
                return null;
            }

            if (string.IsNullOrWhiteSpace(presetName))
            {
                Log.Warn("No preset file content found");
                return null;
            }

            return presetName;
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

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BindingFileMonitor()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
