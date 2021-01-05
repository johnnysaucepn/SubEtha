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

            if (buttonNumber > 0)
            {
                _mouse.XButtonClick(buttonNumber);
            }
        }

        public void Hold(string button)
        {
            int buttonNumber = ParseMouseButtonNumber(button);

            if (buttonNumber > 0)
            {
                _mouse.XButtonDown(buttonNumber);
            }
        }

        public void Release(string button)
        {
            int buttonNumber = ParseMouseButtonNumber(button);

            if (buttonNumber > 0)
            {
                _mouse.XButtonUp(buttonNumber);
            }
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