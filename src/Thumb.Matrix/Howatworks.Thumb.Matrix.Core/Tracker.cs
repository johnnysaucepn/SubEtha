using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class Tracker<T> where T : IState, ICloneable<T>, IStateComparable<T>, new()
    {
        private readonly Subject<T> _subject = new Subject<T>();
        public IObservable<T> Observable => _subject.AsObservable();

        private T _currentState = new T() { TimeStamp = DateTimeOffset.MinValue };

        /// <summary>
        /// Ignore previous information, return new state
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="stateChange"></param>
        public void Replace(DateTimeOffset timestamp, Action<T> stateChange)
        {
            var newState = new T { TimeStamp = timestamp };

            // If handler didn't apply the change, don't update state
            try
            {
                stateChange(newState);
            }
            finally
            {
                if (newState.HasChangedSince(_currentState))
                {
                    _subject.OnNext(newState);
                    _currentState = newState;
                }
            }
        }

        /// <summary>
        /// Based on previous information, return new state
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="stateChange"></param>
        /// <returns></returns>
        public void Modify(DateTimeOffset timestamp, Action<T> stateChange)
        {
            var newState = _currentState.Clone();
            newState.TimeStamp = timestamp;

            // If handler didn't apply the change, don't update state
            try
            {
                stateChange(newState);
            }
            finally
            {
                if (newState.HasChangedSince(_currentState))
                {
                    _subject.OnNext(newState);
                    _currentState = newState;
                }
            }
        }
    }
}