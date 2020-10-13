using System;
using System.Reactive.Subjects;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class Tracker<T> where T : IState, ICloneable<T>, new()
    {
        private readonly Subject<T> _subject = new Subject<T>();

        public T CurrentState { get; private set; } = new T();

        public IObservable<T> Observable => _subject;

        /// <summary>
        /// Ignore previous information, return new state
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="stateChange"></param>
        /// <returns></returns>
        public void Replace(DateTimeOffset timestamp, Func<T, bool> stateChange)
        {
            var newState = new T { TimeStamp = timestamp };

            // If handler didn't apply the change, don't update state
            if (!stateChange(newState)) return;

            CurrentState = newState;
            _subject.OnNext(CurrentState);
        }

        /// <summary>
        /// Based on previous information, return new state
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="stateChange"></param>
        /// <returns></returns>
        public void Modify(DateTimeOffset timestamp, Func<T, bool> stateChange)
        {
            var newState = CurrentState.Clone();
            newState.TimeStamp = timestamp;

            // If handler didn't apply the change, don't update state
            if (!stateChange(newState)) return;

            CurrentState = newState;

            _subject.OnNext(CurrentState);
        }
    }
}