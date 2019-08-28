namespace Howatworks.Thumb.Assistant.Core
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