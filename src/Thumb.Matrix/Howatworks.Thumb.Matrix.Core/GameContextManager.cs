using System;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.SubEtha.Monitor;
using log4net;

namespace Howatworks.Thumb.Matrix.Core
{
    public class GameContextManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameContextManager));

        public string CommanderName { get; private set; }
        public string GameVersion { get; private set; }

        public void SubscribeTo(IObservable<JournalEntry> observable)
        {
            // NOTE: inconsistency in events, some use 'Name', some 'Commander'

            observable.OfJournalType<FileHeader>().Subscribe(h =>
            {
                UpdateGameVersion(h.GameVersion);
            });

            observable.OfJournalType<Commander>().Subscribe(c =>
            {
                UpdateCommander(c.Name);
            });

            observable.OfJournalType<LoadGame>().Subscribe(l =>
            {
                UpdateCommander(l.Commander);
            });

            observable.OfJournalType<ClearSavedGame>().Subscribe(c =>
            {
                UpdateCommander(c.Name);
            });

            observable.OfJournalType<NewCommander>().Subscribe(n =>
            {
                UpdateCommander(n.Name);
            });
        }

        private void UpdateGameVersion(string newGameVersion)
        {
            if (string.IsNullOrEmpty(newGameVersion)) return;
            if (!string.Equals(GameVersion, newGameVersion, StringComparison.InvariantCultureIgnoreCase))
            {
                Log.Info($"Found game version '{newGameVersion}'");
                GameVersion = newGameVersion;
            }
        }

        private void UpdateCommander(string newCmdrName)
        {
            if (string.IsNullOrEmpty(newCmdrName)) return;
            if (!string.Equals(CommanderName, newCmdrName, StringComparison.InvariantCultureIgnoreCase))
            {
                Log.Info($"Found commander '{newCmdrName}'");
                CommanderName = newCmdrName;
            }
        }
    }
}
