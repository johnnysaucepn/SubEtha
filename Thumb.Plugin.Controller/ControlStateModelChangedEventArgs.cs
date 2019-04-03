using Thumb.Plugin.Controller.Messages;

namespace Thumb.Plugin.Controller
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