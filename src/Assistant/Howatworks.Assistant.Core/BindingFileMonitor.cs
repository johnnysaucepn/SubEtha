using Howatworks.SubEtha.Bindings;
using Howatworks.SubEtha.Monitor;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Howatworks.Assistant.Core
{
    public class BindingFileMonitor : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BindingFileMonitor));

        private readonly Dictionary<string, BindingMapper> _allBindingsByPresetName = new Dictionary<string, BindingMapper>();

        private readonly CustomFileWatcher _allBindingFileWatcher;
        private readonly CustomFileWatcher _selectedBindingFileWatcher;
        private string _currentSelectedBindingName;
        private bool disposedValue;

        private IDisposable _allBindingSubscription;
        private IDisposable _selectedBindingSubscription;

        public BindingFileMonitor(IConfiguration config)
        {
            var folder = config["BindingsFolder"];
            _allBindingFileWatcher = new CustomFileWatcher(folder, "*.binds");
            _allBindingSubscription = _allBindingFileWatcher.CreatedFiles
                .Merge(_allBindingFileWatcher.ChangedFiles)
                .Subscribe(x =>
                {
                    var updatedBinding = BindingMapper.FromFile(Path.Combine(folder, x));
                    var presetName = updatedBinding.GetPresetName();
                    _allBindingsByPresetName[presetName] = updatedBinding;
                });

            _selectedBindingFileWatcher = new CustomFileWatcher(folder, "StartPreset.start");
            _selectedBindingSubscription = _selectedBindingFileWatcher.CreatedFiles
                .Merge(_selectedBindingFileWatcher.ChangedFiles)
                .Subscribe(x =>
                {
                    Log.Info("Checking for new binding preset");
                    _currentSelectedBindingName = ReadStartPreset(Path.Combine(folder, x));
                    Log.Info($"Selecting binding preset {_currentSelectedBindingName}");
                });
        }

        public BindingMapper GetCurrentBindingMapper()
        {
            if (_allBindingsByPresetName.TryGetValue(_currentSelectedBindingName, out var selectedBindingMapper))
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
