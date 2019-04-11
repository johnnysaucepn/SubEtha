namespace Thumb.Plugin.Controller.ControlSimulators
{
    public interface IVirtualKeyboardSimulator
    {
        void Activate(string key, params string[] modifierNames);
    }
}