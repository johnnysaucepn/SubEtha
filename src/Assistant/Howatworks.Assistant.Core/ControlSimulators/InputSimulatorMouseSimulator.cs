using InputSimulatorStandard;
using log4net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Howatworks.Assistant.Core.ControlSimulators
{
    [ExcludeFromCodeCoverage]
    public class InputSimulatorMouseSimulator : IVirtualMouseSimulator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InputSimulatorMouseSimulator));

        private readonly IMouseSimulator _mouse = new MouseSimulator();

        private static readonly Regex _extractButtonNumber = new Regex("Mouse_(?<buttonnumber>[0-9]+)");

        public void Activate(string button)
        {
            int buttonNumber = ParseMouseButtonNumber(button);

            if (buttonNumber == 1) _mouse.LeftButtonClick();
            else if (buttonNumber == 2) _mouse.RightButtonClick();
            else if (buttonNumber == 3) _mouse.MiddleButtonClick();
            else if (buttonNumber > 3) _mouse.XButtonClick(buttonNumber - 3);
        }

        public void Hold(string button)
        {
            int buttonNumber = ParseMouseButtonNumber(button);

            if (buttonNumber == 1) _mouse.LeftButtonDown();
            else if (buttonNumber == 2) _mouse.RightButtonDown();
            else if (buttonNumber == 3) _mouse.MiddleButtonDown();
            else if (buttonNumber > 3) _mouse.XButtonDown(buttonNumber - 3);
        }

        public void Release(string button)
        {
            int buttonNumber = ParseMouseButtonNumber(button);

            if (buttonNumber == 1) _mouse.LeftButtonUp();
            else if (buttonNumber == 2) _mouse.RightButtonUp();
            else if (buttonNumber == 3) _mouse.MiddleButtonUp();
            else if (buttonNumber > 3) _mouse.XButtonUp(buttonNumber - 3);
        }

        private int ParseMouseButtonNumber(string button)
        {
            var buttonNumber = 0;
            var buttonNumberMatch = _extractButtonNumber.Match(button);
            if (buttonNumberMatch.Success)
            {
                buttonNumber = Convert.ToInt32(buttonNumberMatch.Groups["buttonnumber"].Value);
            }

            return buttonNumber;
        }
    }
}