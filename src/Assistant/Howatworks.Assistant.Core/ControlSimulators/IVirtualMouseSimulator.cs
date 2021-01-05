namespace Howatworks.Assistant.Core.ControlSimulators
{
    public interface IVirtualMouseSimulator
    {
        void Activate(string button);
        void Hold(string button);
        void Release(string button);
    }
}