using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Core
{
    public class JsonJournalMonitorState : IJournalMonitorState
    {
        private const string StorageFileName = "journalmonitor.json";
        private readonly string _storageFilePath;

        private static readonly ILog Log = LogManager.GetLogger(typeof(JsonJournalMonitorState));

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset
        };

        private readonly Lazy<InMemoryJournalMonitorState> _state;

        public DateTimeOffset? LastEntrySeen => _state.Value?.LastEntrySeen;

        public DateTimeOffset? LastChecked => _state.Value?.LastChecked;

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public JsonJournalMonitorState(IConfiguration config)
        {
            var folder = config.GetValue<string>("JournalMonitorStateFolder");
            _storageFilePath = Path.Combine(folder, StorageFileName);
            _state = new Lazy<InMemoryJournalMonitorState>(Load);
        }

        /// <summary>
        /// Update the internal representation and save
        /// </summary>
        public void Update(DateTimeOffset lastChecked, DateTimeOffset lastEntrySeen)
        {
            _state.Value?.Update(lastChecked, lastEntrySeen);
            Save();
        }

        private InMemoryJournalMonitorState Load()
        {
            try
            {
                var jsonState = File.ReadAllText(_storageFilePath);
                var state = JsonConvert.DeserializeObject<InMemoryJournalMonitorState>(jsonState, _serializerSettings);
                if (state != null) return state;
            }
            catch (IOException)
            {
                Log.Warn($"No '{StorageFileName}' found, creating new file");
            }
            return new InMemoryJournalMonitorState();
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private void Save()
        {
            try
            {
                Directory.GetParent(_storageFilePath).Create();
                File.WriteAllText(_storageFilePath, JsonConvert.SerializeObject(_state.Value, _serializerSettings));
            }
            catch (IOException ex)
            {
                Log.Error(ex);
            }
        }
    }
}