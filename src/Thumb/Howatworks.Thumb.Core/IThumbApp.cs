using System;
using System.Threading;

namespace Howatworks.Thumb.Core
{
    public interface IThumbApp
    {
        void Run(CancellationToken token);
        DateTimeOffset? LastChecked { get; }
        DateTimeOffset? LastEntry { get; }
    }
}