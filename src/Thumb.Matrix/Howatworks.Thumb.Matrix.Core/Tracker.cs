using System;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class Tracker<T> where T:IState, ICloneable<T>,new()
    {
        private readonly CommanderTracker _commander;
        public string GameVersion { get; private set; } = string.Empty;
        public string CommanderName { get; private set; } = string.Empty;
        public T CurrentState { get; private set; } = new T();
        private bool _isDirty;

        public Tracker(CommanderTracker commander)
        {
            _commander = commander;
        }

        /// <summary>
        /// Ignore previous information, return new state
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="stateChange"></param>
        /// <returns></returns>
        public bool Replace(DateTimeOffset timestamp, Func<T, bool> stateChange)
        {
            var newState = new T {TimeStamp = timestamp};

            // If handler didn't apply the change, don't update state
            if (!stateChange(newState)) return false;

            GameVersion = _commander.GameVersion;
            CommanderName = _commander.CommanderName;
            CurrentState = newState;
            _isDirty = true;

            return true;
        }

        /// <summary>
        /// Based on previous information, return new state
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="stateChange"></param>
        /// <returns></returns>
        public bool Modify(DateTimeOffset timestamp, Func<T, bool> stateChange)
        {
            var newState = CurrentState.Clone();
            newState.TimeStamp = timestamp;

            // If handler didn't apply the change, don't update state
            if (!stateChange(newState)) return false;

            GameVersion = _commander.GameVersion;
            CommanderName = _commander.CommanderName;
            CurrentState = newState;
            _isDirty = true;

            return true;
        }

        public void Commit(Action commitAction)
        {
            if (_isDirty) commitAction();
            _isDirty = false;
        }
    }
}