namespace Howatworks.Assistant.Core.ControlSimulators
{
    public class NullKeyboardSimulator : IVirtualKeyboardSimulator
    {
        public void Activate(string key, params string[] modifierNames)
        {
        }

        public void Hold(string key, params string[] modifierNames)
        {
        }

        public void Release(string key, params string[] modifierNames)
        {
        }
    }
}