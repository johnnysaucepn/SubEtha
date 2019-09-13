using System;
using System.Collections.Generic;
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
        private readonly Dictionary<GameContext, SessionState> _sessions = new Dictionary<GameContext, SessionState>();
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
            Replace(loadGame, new SessionState
            {
                CommanderName = loadGame.Commander,
                GameMode = loadGame.GameMode,
                Group = loadGame.Group
            });
            return true;
        }

        private bool ApplyNewCommander(NewCommander newCommander)
        {
            Replace(newCommander, new SessionState
            {
                CommanderName = newCommander.Name
            });
            return true;
        }

        private bool ApplyClearSavedGame(ClearSavedGame clearSavedGame)
        {
            Replace(clearSavedGame, new SessionState
            {
                CommanderName = clearSavedGame.Name
            });
            return true;
        }

        private bool ApplyFileHeader(FileHeader fileHeader)
        {
            Modify(fileHeader, x =>
            {
                x.Build = fileHeader.Build;
                return true;
            });
            return true;
        }

        private void Replace(IJournalEntry entry, SessionState newState)
        {
            var discriminator = _commander.Context;

            newState.TimeStamp = entry.Timestamp;
            _sessions[discriminator] = newState;
            _isDirty = true;
        }

        private void Modify(IJournalEntry entry, Func<SessionState, bool> stateChange)
        {
            var discriminator = _commander.Context;

            var state = !_sessions.ContainsKey(discriminator) ? new SessionState() : _sessions[discriminator];

            // If handler didn't apply the change, don't update state
            if (!stateChange(state)) return;

            state.TimeStamp = entry.Timestamp;
            _sessions[discriminator] = state;
            _isDirty = true;
        }

        private bool BatchComplete()
        {
            if (!_isDirty) return false;

            try
            {
                foreach (var context in _sessions.Keys)
                {
                    _client.Upload(context, _sessions[context]);
                }

                _isDirty = false;
            }
            catch (MatrixUploadException ex)
            {
                // Fail whole operation on any part
                // This may result in multiple uploads, but better than than unsynced data
                Log.Error("Upload error", ex);
            }
            catch (MatrixAuthenticationException ex)
            {
                // Fail whole operation on any part
                // This may result in multiple uploads, but better than than unsynced data
                Log.Error("Authentication error", ex);
                throw;
            }

            return true;
        }

    }
}
