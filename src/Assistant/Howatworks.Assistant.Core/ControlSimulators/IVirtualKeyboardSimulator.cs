namespace Howatworks.Assistant.Core.ControlSimulators
{
    public interface IVirtualKeyboardSimulator
    {
        void Activate(string key, params string[] modifierNames);
        void Hold(string key, params string[] modifierNames);
        void Release(string key, params string[] modifierNames);
    }
}