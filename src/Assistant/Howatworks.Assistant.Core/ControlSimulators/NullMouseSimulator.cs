using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Assistant.Core.ControlSimulators
{
    [ExcludeFromCodeCoverage]
    public class NullMouseSimulator : IVirtualMouseSimulator
    {
        public void Activate(string button)
        {
        }

        public void Hold(string button)
        {
        }

        public void Release(string button)
        {
        }
    }
}