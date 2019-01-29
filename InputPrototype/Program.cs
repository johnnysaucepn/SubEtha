using System;
using System.Runtime.InteropServices;
using System.Threading;
using static PInvoke.User32;

namespace InputPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                var windowTitle = "Elite - Dangerous (CLIENT)";
                //var windowTitle = "Games";
                var hwnd = FindWindow(null, windowTitle);
                if (hwnd != IntPtr.Zero)
                {
                    if (GetForegroundWindow() == hwnd)
                    {
                        Console.Write("X");
                        PressAndRelease(ScanCode.DOWN, true);
                        //PressAndRelease(VirtualKey.VK_DOWN);  // Doesn't work, at least for arrow keys
                    }
                }
                Console.Write(".");
                
                Thread.Sleep(2000);
            } while (true);
        }

        private static void PressAndRelease(ScanCode scanCode, bool extended)
        {
            SendKeyAction(true, scanCode, extended);
            Thread.Sleep(100);
            SendKeyAction(false, scanCode, extended);
        }
        
        private static uint SendKeyAction(bool keyDown, ScanCode scanCode, bool extended)
        {
            var key = new INPUT
            {
                type = InputType.INPUT_KEYBOARD,
                Inputs = new INPUT.InputUnion()
                {
                    ki = new KEYBDINPUT()
                    {
                        dwFlags = KEYEVENTF.KEYEVENTF_SCANCODE |
                                  (extended ? KEYEVENTF.KEYEVENTF_EXTENDED_KEY : 0) |
                                  (keyDown ? 0 : KEYEVENTF.KEYEVENTF_KEYUP),
                        wScan = scanCode
                    }
                }
            };
            var inputs = new[] {key};
            var response = SendInput(inputs.Length, inputs, Marshal.SizeOf(key));
            return response;
        }

        private static void PressAndRelease(VirtualKey virtualKey)
        {
            SendKeyAction(true, virtualKey);
            Thread.Sleep(100);
            SendKeyAction(false, virtualKey);
        }

        private static uint SendKeyAction(bool keyDown, VirtualKey virtualKey)
        {
            var key = new INPUT
            {
                type = InputType.INPUT_KEYBOARD,
                Inputs = new INPUT.InputUnion()
                {
                    ki = new KEYBDINPUT()
                    {
                        dwFlags = (keyDown ? 0 : KEYEVENTF.KEYEVENTF_KEYUP),
                        wVk = virtualKey
                    }
                }
            };
            var inputs = new[] { key };
            var response = SendInput(inputs.Length, inputs, Marshal.SizeOf(key));
            return response;
        }

    }
}
