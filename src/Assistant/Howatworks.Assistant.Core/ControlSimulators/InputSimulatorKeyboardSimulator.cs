using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using InputSimulatorStandard;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Assistant.Core.ControlSimulators
{
    [ExcludeFromCodeCoverage]
    public class InputSimulatorKeyboardSimulator : IVirtualKeyboardSimulator
    {
        private readonly TimeSpan KeyDownTime = TimeSpan.FromMilliseconds(500);

        private static readonly ILog Log = LogManager.GetLogger(typeof(InputSimulatorKeyboardSimulator));

        private readonly IKeyboardSimulator _keyboard = new KeyboardSimulator();
        private readonly VirtualKeyMapper _mapper = new VirtualKeyMapper();

        public InputSimulatorKeyboardSimulator(IConfiguration config)
        {
            var ms = config.GetValue<int?>("KeyDownTimeMs");
            if (ms.HasValue)
            {
                KeyDownTime = TimeSpan.FromMilliseconds(ms.Value);
            }
        }

        public void Activate(string key, params string[] modifierNames)
        {
            var modifiers = _mapper.MapKeys(modifierNames);
            var keyCode = _mapper.MapKey(key);
            if (!keyCode.HasValue)
            {
                Log.Warn($"No mapped key for '{key}'");
                return;
            }

            // IKeyboardSimulator.ModifiedKeyStroke is the standard way to do this, but something about the speed of execution
            // means Elite doesn't always react. Instead, control the sequence of events manually, and add a custom delay.
            foreach (var mod in modifiers) _keyboard.KeyDown(mod);
            _keyboard.KeyDown(keyCode.Value);
            Thread.Sleep(KeyDownTime);
            _keyboard.KeyUp(keyCode.Value);
            foreach (var mod in modifiers) _keyboard.KeyUp(mod);
        }

        public void Hold(string key, params string[] modifierNames)
        {
            var modifiers = _mapper.MapKeys(modifierNames);
            var keyCode = _mapper.MapKey(key);
            if (!keyCode.HasValue)
            {
                Log.Warn($"No mapped key for '{key}'");
                return;
            }

            foreach (var mod in modifiers) _keyboard.KeyDown(mod);
            _keyboard.KeyDown(keyCode.Value);
        }

        public void Release(string key, params string[] modifierNames)
        {
            var modifiers = _mapper.MapKeys(modifierNames);
            var keyCode = _mapper.MapKey(key);
            if (!keyCode.HasValue)
            {
                Log.Warn($"No mapped key for '{key}'");
                return;
            }

            foreach (var mod in modifiers) _keyboard.KeyUp(mod);
            _keyboard.KeyUp(keyCode.Value);
        }
    }
}