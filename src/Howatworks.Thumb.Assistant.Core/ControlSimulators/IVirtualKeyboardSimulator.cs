namespace Howatworks.Thumb.Assistant.Core.ControlSimulators
{
    public interface IVirtualKeyboardSimulator
    {
        void Activate(string key, params string[] modifierNames);
    }
}