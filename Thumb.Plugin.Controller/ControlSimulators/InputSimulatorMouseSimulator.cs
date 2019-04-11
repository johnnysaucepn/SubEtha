using InputSimulatorStandard;
using log4net;

namespace Thumb.Plugin.Controller.ControlSimulators
{
    public class InputSimulatorMouseSimulator : IVirtualMouseSimulator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InputSimulatorMouseSimulator));

        private readonly IMouseSimulator _mouse = new MouseSimulator();

        public void Activate(string button)
        {
            switch (button)
            {
                case "Mouse_1":
                    _mouse.LeftButtonClick();
                    break;
                case "Mouse_2":
                    _mouse.RightButtonClick();
                    break;
            }
        }

    }
}