using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;

namespace InputSimulatorPlusPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = Path.GetTempFileName();

            using (var process = Process.Start("notepad.exe", fileName))
            {
                // Attempts to query process windows handles have failed -
                // use a cruder delay until notepad starts
                Thread.Sleep(1000);

                var keyb = new KeyboardSimulator();

                keyb.TextEntry("Hello World!");
                keyb.ModifiedKeyStroke(VirtualKeyCode.RMENU, VirtualKeyCode.VK_4);
                keyb.ModifiedKeyStroke(null, VirtualKeyCode.BACK);
                keyb.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_S);
                keyb.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.F4);
                keyb.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_L);

                process.WaitForExit();

                var text = File.ReadAllText(fileName);
                Console.WriteLine(text.Equals("Hello World!"));
                File.Delete(fileName);
            }

            Console.ReadKey();


        }
    }
}
