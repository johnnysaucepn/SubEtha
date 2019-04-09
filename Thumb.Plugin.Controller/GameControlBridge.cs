using System;
using System.Linq;
using Howatworks.EliteDangerous.Bindings;
using log4net;
using Microsoft.Extensions.Configuration;
using Thumb.Plugin.Controller.ControlSimulators;
using static PInvoke.User32;

namespace Thumb.Plugin.Controller
{
    public class GameControlBridge
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameControlBridge));

        private readonly string _activeWindowTitle;

        private readonly IKeyboardSimulator _keyboard;
        private readonly IMouseSimulator _mouse;

        public GameControlBridge(IConfiguration configuration, IKeyboardSimulator keyboard, IMouseSimulator mouse)
        {
            _keyboard = keyboard;
            _mouse = mouse;
            _activeWindowTitle = configuration["ActiveWindowTitle"];
        }

        public void TriggerKeyCombination(Button button)
        {

            Button.ButtonBinding selectedButtonBinding;
            if (button.Primary.Device == "Keyboard")
            {
                selectedButtonBinding = button.Primary;
            }
            else if (button.Secondary.Device == "Keyboard")
            {
                selectedButtonBinding = button.Secondary;
            }
            else
            {
                Log.Warn($"Neither primary or secondary bindings are for keyboard (found {button.Primary.Device}, {button.Primary.Device}");
                return;
            }

            var modifierNames = selectedButtonBinding.Modifier.Select(x => x.Key).ToArray();
            Log.Info($"Pressing {selectedButtonBinding.Key} with {(modifierNames.Any() ? string.Join(", ", modifierNames) : "no")} modifiers");
            if (DoesWindowHaveFocus(_activeWindowTitle))
            {
                _keyboard.Activate(selectedButtonBinding.Key, modifierNames);
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