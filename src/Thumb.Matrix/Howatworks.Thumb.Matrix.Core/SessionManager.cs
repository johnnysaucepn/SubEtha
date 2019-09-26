using System;
using System.Collections.Concurrent;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.Thumb.Core;
using log4net;

namespace Howatworks.Thumb.Matrix.Core
{
    public class SessionManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationManager));

        private readonly CommanderTracker _commander;
        private readonly IUploader<SessionState> _client;
        private readonly ConcurrentDictionary<GameContext, SessionState> _sessions = new ConcurrentDictionary<GameContext, SessionState>();
        private bool _isDirty;

        public SessionManager(JournalEntryRouter router, CommanderTracker commander, IUploader<SessionState> client)
        {
            _commander = commander;
            _client = client;

            router.RegisterFor<LoadGame>(ApplyLoadGame);
            router.RegisterFor<NewCommander>(ApplyNewCommander);
            router.RegisterFor<ClearSavedGame>(ApplyClearSavedGame);
            router.RegisterFor<FileHeader>(ApplyFileHeader);

            router.RegisterForBatchComplete(BatchComplete);
        }

        private bool ApplyLoadGame(LoadGame loadGame)
        {
            return Replace(loadGame.Timestamp, new SessionState
            {
                CommanderName = loadGame.Commander,
                GameMode = loadGame.GameMode,
                Group = loadGame.Group
            });
        }

        private bool ApplyNewCommander(NewCommander newCommander)
        {
            return Replace(newCommander.Timestamp, new SessionState
            {
                CommanderName = newCommander.Name
            });
        }

        private bool ApplyClearSavedGame(ClearSavedGame clearSavedGame)
        {
            return Replace(clearSavedGame.Timestamp, new SessionState
            {
                CommanderName = clearSavedGame.Name
            });
        }

        private bool ApplyFileHeader(FileHeader fileHeader)
        {
            return Modify(fileHeader.Timestamp, x =>
            {
                x.Build = fileHeader.Build;
                return true;
            });
        }

        private bool Replace(DateTimeOffset timestamp, SessionState newState)
        {
            var discriminator = _commander.Context;
            if (string.IsNullOrWhiteSpace(discriminator.CommanderName)) return false;
            if (string.IsNullOrWhiteSpace(discriminator.GameVersion)) return false;

            newState.TimeStamp = timestamp;
            _sessions[discriminator] = newState;
            _isDirty = true;
            return true;
        }

        private bool Modify(DateTimeOffset timestamp, Func<SessionState, bool> stateChange)
        {
            var discriminator = _commander.Context;
            if (string.IsNullOrWhiteSpace(discriminator.CommanderName)) return false;
            if (string.IsNullOrWhiteSpace(discriminator.GameVersion)) return false;

            var state = _sessions.ContainsKey(discriminator) ? _sessions[discriminator] : new SessionState();

            // If handler didn't apply the change, don't update state
            if (!stateChange(state)) return false;

            state.TimeStamp = timestamp;
            _sessions[discriminator] = state;
            _isDirty = true;
            return true;
        }

        private bool BatchComplete()
        {
            if (!_isDirty) return false;

            foreach (var context in _sessions)
            {
                _client.Upload(context.Key, context.Value);
            }

            _isDirty = false;

            return true;
        }
    }
}
