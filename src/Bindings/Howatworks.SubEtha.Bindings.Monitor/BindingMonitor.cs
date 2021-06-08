using Howatworks.SubEtha.Common;
using Howatworks.SubEtha.Common.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Howatworks.SubEtha.Bindings.Monitor
{
    public class BindingMonitor
    {
        private static readonly SubEthaLog Log = SubEthaLog.GetLogger<BindingMonitor>();

        private readonly CustomFileWatcher _allBindingFileWatcher;
        private readonly CustomFileWatcher _selectedBindingFileWatcher;

        public IObservable<BindingSet> BindingsChanged { get; }
        public IObservable<SelectedPresets> SelectedPresetChanged { get; }

        public BindingMonitor(IConfiguration config)
        {
            var folder = config["BindingsFolder"];

            // Watch for any new binding files coming in or being changed
            _allBindingFileWatcher = new CustomFileWatcher(folder, "*.binds");

            // When they do, make sure we've read and parsed the file and add it to our list.
            // This lets us switch as soon as the value in StartPreset.start changes, without
            // having to scan all the files again to find the one with that name.
            BindingsChanged = _allBindingFileWatcher.CreatedFiles
                .Merge(_allBindingFileWatcher.ChangedFiles)
                .Select(x =>
                {
                    // Load the .binds file
                    // Normally, we would just notify of the preset name and let the consumer read the file.
                    // However, we can't be sure that a file called, say, 'MyBindings.4.0.binds' actually contains
                    // a preset called 'MyBindings' without parsing the file anyway.
                    // So, we can either return the path and make the consumer read the file, or keep the file
                    // handling at this level and load the file.
                    var path = new FileInfo(Path.Combine(folder, x));
                    return new BindingSetReader(path).Read();
                });

            // Now start raising file events
            _allBindingFileWatcher.Start();

            // Watch for a new preset being selected
            _selectedBindingFileWatcher = new CustomFileWatcher(folder, "StartPreset.start");

            // When it does, switch to that binding set, and notify clients
            SelectedPresetChanged = _selectedBindingFileWatcher.CreatedFiles
                .Merge(_selectedBindingFileWatcher.ChangedFiles)
                // FileWatcher can still produce multiple notifications for the same thing. For that matter,
                // users may flick through a few selections before setting. Throttle the updates.
                .Throttle(TimeSpan.FromSeconds(3))
                .Select(x =>
                {
                    Log.Info("Checking for new binding presets");
                    var currentSelectedPresetNames = ReadStartPresets(new FileInfo(Path.Combine(folder, x)));
                    Log.Info($"Selecting binding presets {currentSelectedPresetNames}");
                    return currentSelectedPresetNames;
                });

            // Now start raising file events
            _selectedBindingFileWatcher.Start();
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
                    [BindingCategory.General] = presetNames.Count > 0 ? presetNames[0] : null,
                    [BindingCategory.InShip] = presetNames.Count > 1 ? presetNames[1] : null,
                    [BindingCategory.Driving] = presetNames.Count > 2 ? presetNames[2] : null,
                    [BindingCategory.OnFoot] = presetNames.Count > 3 ? presetNames[3] : null
                };
            }
            catch (IOException e)
            {
                Log.Warn("Could not read preset", e);
                return null;
            }
        }
    }
}
