using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Howatworks.SubEtha.Monitor;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Core
{
    public class JsonJournalMonitorState : IJournalMonitorState
    {
        private const string StorageFileName = @"journalmonitor.json";
        private readonly string _storageFilePath;

        private static readonly ILog Log = LogManager.GetLogger(typeof(JsonJournalMonitorState));

        private readonly JsonSerializer _serializer = new JsonSerializer
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        private Lazy<InMemoryJournalMonitorState> _state;

        /// <summary>
        /// Update the internal representation and save
        /// </summary>
        public DateTimeOffset? LastRead
        {
            get => _state.Value.LastRead;
            set
            {
                _state.Value.LastRead = value;
                Save();
            }
        }

        /// <summary>
        /// Update the internal representation and save
        /// </summary>
        public DateTimeOffset? LastChecked
        {
            get => _state.Value.LastChecked;
            set
            {
                _state.Value.LastChecked = value;
                Save();
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public JsonJournalMonitorState(IConfiguration config)
        {
            var folder = config.GetValue<string>("JournalMonitorStateFolder");
            _storageFilePath = Path.Combine(folder, StorageFileName);
            _state = new Lazy<InMemoryJournalMonitorState>(Load);
        }

        private InMemoryJournalMonitorState Load()
        {
            try
            {
                using (var stream = File.OpenRead(_storageFilePath))
                using (var reader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return _serializer.Deserialize<InMemoryJournalMonitorState>(jsonReader);
                }
            }
            catch (IOException ex)
            {
                Log.Warn(ex);
                return new InMemoryJournalMonitorState();
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private void Save()
        {

            try
            {
                using (var stream = File.OpenWrite(_storageFilePath))
                using (var writer = new StreamWriter(stream))
                {
                    _serializer.Serialize(writer, _state);
                }
            }
            catch (IOException ex)
            {
                Log.Warn(ex);
                _state = null;
            }
        }
    }
}