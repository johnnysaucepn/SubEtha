namespace Thumb.Plugin.Controller.ControlSimulators
{
    public class NullKeyboardSimulator : IVirtualKeyboardSimulator
    {
        public void Activate(string key, params string[] modifierNames)
        {
        }
    }
}