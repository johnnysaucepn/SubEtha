using System;

namespace Howatworks.Thumb.Core
{
    public interface IThumbApp
    {
        void Initialize();
        void Shutdown();
        DateTimeOffset? LastChecked { get; }
        DateTimeOffset? LastEntry { get; }
        void StartMonitoring();
        void StopMonitoring();
    }
}