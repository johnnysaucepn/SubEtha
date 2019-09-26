using System;

namespace Howatworks.Thumb.Core
{
    public interface IThumbApp
    {
        void Initialize();
        void Shutdown();
        DateTimeOffset? LastChecked();
        DateTimeOffset? LastEntry();
        void StartMonitoring();
        void StopMonitoring();
    }
}