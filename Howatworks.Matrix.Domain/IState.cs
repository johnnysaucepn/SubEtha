using System;

namespace Howatworks.Matrix.Domain
{
    public interface IState
    {
        DateTimeOffset TimeStamp { get; set; }
    }
}
