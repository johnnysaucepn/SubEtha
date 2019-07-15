using System;
using System.Collections.Generic;
using System.Text;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class CommanderTracker
    {
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
                CommanderName = e.Name;
                return true;
            });
            router.RegisterFor<LoadGame>(e =>
            {
                CommanderName = e.Commander;
                return true;
            });
            router.RegisterFor<ClearSavedGame>(e =>
            {
                CommanderName = e.Name;
                return true;
            });
            router.RegisterFor<NewCommander>(e =>
            {
                CommanderName = e.Name;
                return true;
            });
        }
    }
}
