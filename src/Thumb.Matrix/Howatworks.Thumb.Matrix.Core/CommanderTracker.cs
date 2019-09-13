using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.Thumb.Core;
using log4net;

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
                GameVersion = e.GameVersion;
                return true;
            });

            router.RegisterFor<Commander>(e =>
            {
                CommanderName = CommanderName ?? e.Name;
                return true;
            });
            router.RegisterFor<LoadGame>(e =>
            {
                CommanderName = CommanderName ?? e.Commander;
                return true;
            });
            router.RegisterFor<ClearSavedGame>(e =>
            {
                CommanderName = CommanderName ?? e.Name;
                return true;
            });
            router.RegisterFor<NewCommander>(e =>
            {
                CommanderName = CommanderName ?? e.Name;
                return true;
            });
        }
    }
}
