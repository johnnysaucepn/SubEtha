using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace InputPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new InputSimulator();
            do
            {
                input.Keyboard.KeyPress(VirtualKeyCode.VK_K);
                Thread.Sleep(5000);
            } while (true);
        }
    }
}
