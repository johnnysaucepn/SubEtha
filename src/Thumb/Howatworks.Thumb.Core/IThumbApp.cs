using System;

namespace Howatworks.Thumb.Core
{
    public interface IThumbApp
    {
        void Initialize();
        DateTimeOffset? LastChecked();
        DateTimeOffset? LastEntry();
        void Start();
        void Stop();
    }
}