using System;

namespace Howatworks.Matrix.Domain
{
    public interface IStateComparable<T> where T : IState
    {
        bool HasChangedSince(T state);
    }
}
