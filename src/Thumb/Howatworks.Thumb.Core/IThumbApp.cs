using System;
using System.Threading;

namespace Howatworks.Thumb.Core
{
    public interface IThumbApp
    {
        void Run(CancellationToken token);
        void Shutdown();
        DateTimeOffset? LastChecked { get; }
        DateTimeOffset? LastEntry { get; }
        void StartMonitoring();
        void StopMonitoring();
    }
}