using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;
using static PInvoke.User32;

namespace InputPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            

            var input = new InputSimulator();
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
                        //input.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                        var downKey = new INPUT
                        {
                            type = InputType.INPUT_KEYBOARD,
                            Inputs = new INPUT.InputUnion()
                                {ki = new KEYBDINPUT() {wScan = ScanCode.DOWN, wVk = VirtualKey.VK_DOWN}}
                        };
                        var inputs = new[]
                        {
                            downKey,
                        };
                        PInvoke.User32.SendInput(inputs.Length, inputs, Marshal.SizeOf(downKey));
                    }
                }
                Console.Write(".");
                
                Thread.Sleep(2000);
            } while (true);
        }
    }
}
