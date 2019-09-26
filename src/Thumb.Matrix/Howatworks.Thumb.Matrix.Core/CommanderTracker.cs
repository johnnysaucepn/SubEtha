using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.Thumb.Core;
using log4net;
using System;

namespace Howatworks.Thumb.Matrix.Core
{
    public class CommanderTracker
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationManager));

        public string CommanderName { get; private set; }
        public string GameVersion { get; set; }
        public GameContext Context => new GameContext(GameVersion, CommanderName);

        public CommanderTracker(JournalEntryRouter router)
        {
            // NOTE: inconsistency in events, some use 'Name', some 'Commander'

            router.RegisterFor<FileHeader>(e =>
            {
                UpdateGameVersion(e.GameVersion);
                return true;
            });

            router.RegisterFor<Commander>(e =>
            {
                UpdateCommander(e.Name);
                return true;
            });
            router.RegisterFor<LoadGame>(e =>
            {
                UpdateCommander(e.Commander);
                return true;
            });
            router.RegisterFor<ClearSavedGame>(e =>
            {
                UpdateCommander(e.Name);
                return true;
            });
            router.RegisterFor<NewCommander>(e =>
            {
                UpdateCommander(e.Name);
                return true;
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
