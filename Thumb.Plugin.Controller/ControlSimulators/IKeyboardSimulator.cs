namespace Thumb.Plugin.Controller.ControlSimulators
{
    public interface IKeyboardSimulator
    {
        void Activate(string key, params string[] modifierNames);
    }
}