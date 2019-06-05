namespace Howatworks.Thumb.Plugin.Assistant.ControlSimulators
{
    public interface IVirtualKeyboardSimulator
    {
        void Activate(string key, params string[] modifierNames);
    }
}