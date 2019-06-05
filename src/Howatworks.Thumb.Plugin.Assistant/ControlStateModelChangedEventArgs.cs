using Howatworks.Thumb.Plugin.Assistant.Messages;

namespace Howatworks.Thumb.Plugin.Assistant
{
    public class ControlStateModelChangedEventArgs
    {
        public readonly ControlStateModel State;

        public ControlStateModelChangedEventArgs(ControlStateModel state)
        {
            State = state;
        }
    }
}