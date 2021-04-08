using System;
using System.Linq;
using log4net;
using Microsoft.Extensions.Configuration;
using Howatworks.Assistant.Core.ControlSimulators;
using static PInvoke.User32;
using Howatworks.SubEtha.Bindings;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

namespace Howatworks.Assistant.Core
{
    public class GameControlBridge
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameControlBridge));

        private readonly string _activeWindowTitle;

        private readonly IBindingMapper _bindingMapper;
        private readonly IVirtualKeyboardSimulator _keyboard;
        private readonly IVirtualMouseSimulator _mouse;

        public readonly IObservable<Unit> SelectedBindingsChanged;

        public GameControlBridge(IConfiguration configuration, IBindingMapper mapper, IVirtualKeyboardSimulator keyboard, IVirtualMouseSimulator mouse)
        {
            _bindingMapper = mapper;
            _keyboard = keyboard;
            _mouse = mouse;
            _activeWindowTitle = configuration["ActiveWindowTitle"];

            SelectedBindingsChanged =
                Observable.FromEventPattern(
                    h => _bindingMapper.BindingsChanged += h,
                    h => _bindingMapper.BindingsChanged -= h
                )
                .Select(_ => Unit.Default);
        }

        public IReadOnlyCollection<BoundButton> GetAllBoundButtons()
        {
            return _bindingMapper?.GetBoundButtons("Keyboard", "Mouse") ?? new List<BoundButton>();
        }

        public void ActivateKeyCombination(string bindingName)
        {
            var button = LookupButton(bindingName);
            if (button != null)
            {
                TriggerKeyCombination(button, _keyboard.Activate, _mouse.Activate);
            }
        }

        public void ActivateKeyCombination(Button button)
        {
            TriggerKeyCombination(button, _keyboard.Activate, _mouse.Activate);
        }

        public void HoldKeyCombination(string bindingName)
        {
            var button = LookupButton(bindingName);
            if (button != null)
            {
                TriggerKeyCombination(button, _keyboard.Hold, _mouse.Hold);
            }
        }

        public void HoldKeyCombination(Button button)
        {
            TriggerKeyCombination(button, _keyboard.Hold, _mouse.Hold);
        }

        public void ReleaseKeyCombination(string bindingName)
        {
            var button = LookupButton(bindingName);
            if (button != null)
            {
                TriggerKeyCombination(button, _keyboard.Release, _mouse.Release);
            }
        }

        public void ReleaseKeyCombination(Button button)
        {
            TriggerKeyCombination(button, _keyboard.Release, _mouse.Release);
        }

        private Button LookupButton(string bindingName)
        {
            var button = _bindingMapper?.GetButtonBindingByName(bindingName);
            if (button != null)
            {
                return button;
            }
            Log.Warn($"Unknown binding name found: '{bindingName}'");
            return null;
        }

        private void TriggerKeyCombination(Button button, Action<string, string[]> keyboardAction, Action<string> mouseAction)
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
                Log.Warn($"Neither primary or secondary bindings are for keyboard or mouse (found '{button.Primary.Device}', '{button.Secondary.Device}'");
                return;
            }

            var modifierNames = selectedButtonBinding.Modifier.Select(x => x.Key);
            Log.Info($"Pressing '{selectedButtonBinding.Key}' with '{(modifierNames.Any() ? string.Join(", ", modifierNames) : "no")}' modifiers");

            if (selectedButtonBinding.Device == "Keyboard")
            {
                keyboardAction(selectedButtonBinding.Key, modifierNames.ToArray());
            }
            else if (selectedButtonBinding.Device == "Mouse")
            {
                // Expect no modifiers for mouse controls - change if required
                mouseAction(selectedButtonBinding.Key);
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