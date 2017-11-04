using System;
using System.IO;
using Howatworks.PlayerJournal.Parser;
using log4net;
using Newtonsoft.Json;

namespace Thumb.Core
{
    public class JournalMonitorConfiguration : IJournalMonitorConfiguration
    {
        private const string ConfigFile = @"journalmonitor.json";

        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalMonitorConfiguration));

        private readonly JsonSerializer _serialiser = new JsonSerializer
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public string JournalPattern { get; }
        public string JournalFolder { get; }
        public TimeSpan UpdateInterval { get; }

        private JournalMonitorState _state;

        public DateTime? LastRead
        {
            get
            {
                return _state?.LastRead;
            }
            set
            {
                if (_state == null)
                {
                    _state = new JournalMonitorState();
                }
                _state.LastRead = value;
                Save();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public JournalMonitorConfiguration()
        {
            // TODO: config-ise these, and find cross-platform way to detect default properly
            JournalPattern = "Journal.*.log";
            JournalFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Saved Games", "Frontier Developments", "Elite Dangerous");
            UpdateInterval = new TimeSpan(0, 0, 5);

            try
            {
                using (var stream = File.OpenRead(ConfigFile))
                using (var reader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    _state = _serialiser.Deserialize<JournalMonitorState>(jsonReader);
                }
            }
            catch (IOException ex)
            {
                Log.Warn(ex);
                _state = null;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private void Save()
        {

            try
            {
                using (var stream = File.OpenWrite(ConfigFile))
                using (var writer = new StreamWriter(stream))
                {
                    _serialiser.Serialize(writer, _state);
                }
            }
            catch (IOException ex)
            {
                Log.Warn(ex);
                _state = null;
            }
        }
    }

    public class JournalMonitorState
    {
        public DateTime? LastRead { get; set; }
    }
}