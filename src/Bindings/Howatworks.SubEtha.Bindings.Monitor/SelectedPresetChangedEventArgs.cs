namespace Howatworks.SubEtha.Bindings.Monitor
{
    public class SelectedPresetChangedEventArgs
    {
        public SelectedPresets NewSelectedPresets { get; }

        public SelectedPresetChangedEventArgs(SelectedPresets newSelectedPresets)
        {
            NewSelectedPresets = newSelectedPresets;
        }
    }
}