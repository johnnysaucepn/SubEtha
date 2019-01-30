using System;

namespace SubEtha.Domain
{
    public interface IState
    {
        DateTimeOffset TimeStamp { get; set; }
    }
}
