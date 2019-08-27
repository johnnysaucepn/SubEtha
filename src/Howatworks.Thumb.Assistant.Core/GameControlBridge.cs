using System;
using System.Linq;
using log4net;
using Microsoft.Extensions.Configuration;
using Howatworks.Thumb.Assistant.Core.ControlSimulators;
using static PInvoke.User32;
using Howatworks.SubEtha.Bindings;

namespace Howatworks.Thumb.Assistant.Core
{
    public class GameControlBridge
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameControlBridge));

        private readonly string _activeWindowTitle;

        private readonly IVirtualKeyboardSimulator _keyboard;
        private readonly IVirtualMouseSimulator _mouse;

        public GameControlBridge(IConfiguration configuration, IVirtualKeyboardSimulator keyboard, IVirtualMouseSimulator mouse)
        {
            _keyboard = keyboard;
            _mouse = mouse;
            _activeWindowTitle = configuration["ActiveWindowTitle"];
        }

        public void TriggerKeyCombination(Button button)
        {
            if (!DoesWindowHaveFocus(_activeWindowTitle))
            {
                return;
            }

            Button.ButtonBinding selectedButtonBinding;
            if (button.Primary.Device == "Keyboard")
            {
                selectedButtonBinding = button.Primary;
            }
            else if (button.Secondary.Device == "Keyboard")
            {
                selectedButtonBinding = button.Secondary;
            }
            else if (button.Primary.Device == "Mouse")
            {
                selectedButtonBinding = button.Primary;
            }
            else if (button.Secondary.Device == "Mouse")
            {
                selectedButtonBinding = button.Secondary;
            }
            else
            {
                Log.Warn($"Neither primary or secondary bindings are for keyboard or mouse (found {button.Primary.Device}, {button.Secondary.Device}");
                return;
            }

            var modifierNames = selectedButtonBinding.Modifier.Select(x => x.Key).ToArray();
            Log.Info($"Pressing {selectedButtonBinding.Key} with {(modifierNames.Any() ? string.Join(", ", modifierNames) : "no")} modifiers");

            if (selectedButtonBinding.Device == "Keyboard")
            {
                _keyboard.Activate(selectedButtonBinding.Key, modifierNames);
            }
            else if (selectedButtonBinding.Device == "Mouse")
            {
                // Expect no modifiers for mouse controls - change if required
                _mouse.Activate(selectedButtonBinding.Key);
            }
        }

        private static bool DoesWindowHaveFocus(string windowTitle)
        {
            var hwnd = FindWindow(null, windowTitle);
            if (hwnd == IntPtr.Zero)
            {
                Log.Debug($"Window '{windowTitle}' not found");
                return false;
            }

            if (GetForegroundWindow() == hwnd)
            {
                return true;
            }

            Log.Debug($"Window '{windowTitle}' not in foreground");
            return false;
        }


    }
}