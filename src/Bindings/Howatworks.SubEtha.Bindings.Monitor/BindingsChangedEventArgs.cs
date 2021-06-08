using System;

namespace Howatworks.SubEtha.Bindings.Monitor
{
    public class BindingsChangedEventArgs : EventArgs
    {
        public string PresetName { get; }

        public BindingsChangedEventArgs(string presetName)
        {
            PresetName = presetName;
        }
    }
}