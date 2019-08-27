using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using PInvoke;

namespace Howatworks.Thumb.Assistant.Core.ControlSimulators
{
    public class SimpleKeyboardSimulator : IVirtualKeyboardSimulator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SimpleKeyboardSimulator));

        private readonly SimpleKeyMappingTable _keyMapping = new SimpleKeyMappingTable();

        public void Activate(string key, params string[] modifierNames)
        {
            var modifiers = MapModifiers(modifierNames);
            var mainKey = MapKey(key);

            Press(modifiers);
            PressAndRelease(mainKey);
            Release(modifiers);

        }

        private User32.ScanCode MapKey(string key)
        {
            return _keyMapping.GetScanCode(key);
        }

        private User32.VirtualKey[] MapModifiers(IEnumerable<string> modifiers)
        {
            return modifiers.Select(x => _keyMapping.GetVirtualKey(x)).ToArray();
        }

        private static void PressAndRelease(params User32.ScanCode[] scanCodes)
        {
            Press(scanCodes);
            Thread.Sleep(100);
            Release(scanCodes);
        }

        private static void PressAndRelease(params User32.VirtualKey[] virtualKeys)
        {
            Press(virtualKeys);
            Thread.Sleep(100);
            Release(virtualKeys);
        }

        private static void Press(params User32.ScanCode[] scanCodes)
        {
            foreach (var scanCode in scanCodes)
            {
                if (scanCode == User32.ScanCode.NONAME)
                {
                    Log.Error($"Invalid ScanCode {scanCode}");
                    continue;
                }

                SendKeyAction(true, scanCode, true);

            }
        }

        private static void Release(params User32.ScanCode[] scanCodes)
        {
            foreach (var scanCode in scanCodes)
            {
                if (scanCode == User32.ScanCode.NONAME)
                {
                    if (scanCode == User32.ScanCode.NONAME)
                    {
                        Log.Error($"Invalid ScanCode {scanCode}");
                        continue;
                    }
                }

                SendKeyAction(false, scanCode, true);
            }
        }

        private static void Press(params User32.VirtualKey[] virtualKeys)
        {
            foreach (var virtualKey in virtualKeys)
            {
                if (virtualKey == User32.VirtualKey.VK_NONAME)
                {
                    Log.Error($"Invalid VirtualKey {virtualKey}");
                    continue;
                }

                SendKeyAction(true, virtualKey);
            }
        }

        private static void Release(params User32.VirtualKey[] virtualKeys)
        {
            foreach (var virtualKey in virtualKeys)
            {
                if (virtualKey == User32.VirtualKey.VK_NONAME)
                {
                    Log.Error($"Invalid VirtualKey {virtualKey}");
                    continue;
                }

                SendKeyAction(false, virtualKey);
            }
        }

        [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
        private static uint SendKeyAction(bool keyDown, User32.ScanCode scanCode, bool extended)
        {
            var key = new User32.INPUT
            {
                type = User32.InputType.INPUT_KEYBOARD,
                Inputs = new User32.INPUT.InputUnion()
                {
                    ki = new User32.KEYBDINPUT()
                    {
                        dwFlags = User32.KEYEVENTF.KEYEVENTF_SCANCODE |
                                  (extended ? User32.KEYEVENTF.KEYEVENTF_EXTENDED_KEY : 0) |
                                  (keyDown ? 0 : User32.KEYEVENTF.KEYEVENTF_KEYUP),
                        wScan = scanCode
                    }
                }
            };
            var inputs = new[] { key };
            var response = User32.SendInput(inputs.Length, inputs, Marshal.SizeOf(key));
            return response;
        }

        [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
        private static uint SendKeyAction(bool keyDown, User32.VirtualKey virtualKey)
        {
            var key = new User32.INPUT
            {
                type = User32.InputType.INPUT_KEYBOARD,
                Inputs = new User32.INPUT.InputUnion()
                {
                    ki = new User32.KEYBDINPUT()
                    {
                        dwFlags = (keyDown ? 0 : User32.KEYEVENTF.KEYEVENTF_KEYUP),
                        wVk = virtualKey
                    }
                }
            };
            var inputs = new[] { key };
            var response = User32.SendInput(inputs.Length, inputs, Marshal.SizeOf(key));
            return response;
        }
    }
}