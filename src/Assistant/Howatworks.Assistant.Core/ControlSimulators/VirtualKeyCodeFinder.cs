using InputSimulatorStandard.Native;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Howatworks.Assistant.Core.ControlSimulators
{
    /// <summary>
    /// See http://www.pinvoke.net/default.aspx/user32.vkkeyscanex
    /// </summary>
    public class VirtualKeyCodeFinder : IDisposable
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern short VkKeyScanEx(char ch, IntPtr dwhkl);
        [DllImport("user32.dll")]
        private static extern bool UnloadKeyboardLayout(IntPtr hkl);
        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        private readonly IntPtr pointer;

        public VirtualKeyCodeFinder(int klid)
        {
            pointer = LoadKeyboardLayout(klid.ToString("X8"), 1);
        }

        public VirtualKeyCodeFinder(CultureInfo culture) : this(culture.KeyboardLayoutId)
        {
        }

        public void Dispose()
        {
            UnloadKeyboardLayout(pointer);
            GC.SuppressFinalize(this);
        }

        ~VirtualKeyCodeFinder()
        {
            UnloadKeyboardLayout(pointer);
        }

        public bool TryGetKey(char character, out VirtualKeyCode key)
        {
            short keyNumber = VkKeyScanEx(character, pointer);
            if (keyNumber == -1)
            {
                key = 0;
                return false;
            }
            // "If the function succeeds, the low-order byte of the return value contains the virtual-key code
            // and the high-order byte contains the shift state."
            key = (VirtualKeyCode)(keyNumber & 0xFF);
            return true;
        }

        public VirtualKeyCode GetKeyOrDefault(char character, VirtualKeyCode defaultKeyCode)
        {
            return TryGetKey(character, out var key) ? key : defaultKeyCode;
        }
    }
}
