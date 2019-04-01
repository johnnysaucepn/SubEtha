namespace Thumb.Plugin.Controller
{
    public class ControllerModeUpdateEventArgs
    {
        public readonly GameStatus Status;

        public ControllerModeUpdateEventArgs(GameStatus status)
        {
            Status = status;
        }
    }
}