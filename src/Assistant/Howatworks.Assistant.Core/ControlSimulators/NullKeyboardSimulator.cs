using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Assistant.Core.ControlSimulators
{
    [ExcludeFromCodeCoverage]
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