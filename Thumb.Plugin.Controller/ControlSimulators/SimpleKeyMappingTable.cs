using System.Collections.Generic;
using static PInvoke.User32;

namespace Thumb.Plugin.Controller.ControlSimulators
{
    public class SimpleKeyMappingTable
    {
        /// <summary>
        /// Sourced from https://github.com/richardbuckle/EDRefCard/blob/1f1d65c3f55eda18d1923ee8175026125fd65e44/bindings/testCases/Help.txt
        /// </summary>
        public readonly IReadOnlyDictionary<string, (ScanCode, VirtualKey)> Table = new Dictionary<string, (ScanCode, VirtualKey)>
        {
            ["Key_Escape"] = (ScanCode.ESCAPE, VirtualKey.VK_ESCAPE),
            ["Key_1"] = (ScanCode.KEY_1, VirtualKey.VK_KEY_1),
            ["Key_2"] = (ScanCode.KEY_2, VirtualKey.VK_KEY_2),
            ["Key_3"] = (ScanCode.KEY_3, VirtualKey.VK_KEY_3),
            ["Key_4"] = (ScanCode.KEY_4, VirtualKey.VK_KEY_4),
            ["Key_5"] = (ScanCode.KEY_5, VirtualKey.VK_KEY_5),
            ["Key_6"] = (ScanCode.KEY_6, VirtualKey.VK_KEY_6),
            ["Key_7"] = (ScanCode.KEY_7, VirtualKey.VK_KEY_7),
            ["Key_8"] = (ScanCode.KEY_8, VirtualKey.VK_KEY_8),
            ["Key_9"] = (ScanCode.KEY_9, VirtualKey.VK_KEY_9),
            ["Key_0"] = (ScanCode.KEY_0, VirtualKey.VK_KEY_0),
            ["Key_Minus"] = (ScanCode.OEM_MINUS, VirtualKey.VK_OEM_MINUS),
            ["Key_Equals"] = (ScanCode.OEM_PLUS, VirtualKey.VK_OEM_PLUS), // TODO: unmodified
            ["Key_Backspace"] = (ScanCode.BACK, VirtualKey.VK_BACK),
            ["Key_Tab"] = (ScanCode.TAB, VirtualKey.VK_TAB),
            ["Key_Q"] = (ScanCode.KEY_Q, VirtualKey.VK_Q),
            ["Key_W"] = (ScanCode.KEY_W, VirtualKey.VK_W),
            ["Key_E"] = (ScanCode.KEY_E, VirtualKey.VK_E),
            ["Key_R"] = (ScanCode.KEY_R, VirtualKey.VK_R),
            ["Key_T"] = (ScanCode.KEY_T, VirtualKey.VK_T),
            ["Key_Y"] = (ScanCode.KEY_Y, VirtualKey.VK_Y),
            ["Key_U"] = (ScanCode.KEY_U, VirtualKey.VK_U),
            ["Key_I"] = (ScanCode.KEY_I, VirtualKey.VK_I),
            ["Key_O"] = (ScanCode.KEY_O, VirtualKey.VK_O),
            ["Key_P"] = (ScanCode.KEY_P, VirtualKey.VK_P),
            ["Key_LeftBracket"] = (ScanCode.OEM_4, VirtualKey.VK_OEM_4), // { US
            ["Key_RightBracket"] = (ScanCode.OEM_6, VirtualKey.VK_OEM_6), // } US
            ["Key_Enter"] = (ScanCode.RETURN, VirtualKey.VK_RETURN),
            ["Key_LeftControl"] = (ScanCode.LCONTROL, VirtualKey.VK_LCONTROL),
            ["Key_A"] = (ScanCode.KEY_A, VirtualKey.VK_A),
            ["Key_S"] = (ScanCode.KEY_S, VirtualKey.VK_S),
            ["Key_D"] = (ScanCode.KEY_D, VirtualKey.VK_D),
            ["Key_F"] = (ScanCode.KEY_F, VirtualKey.VK_F),
            ["Key_G"] = (ScanCode.KEY_G, VirtualKey.VK_G),
            ["Key_H"] = (ScanCode.KEY_H, VirtualKey.VK_H),
            ["Key_J"] = (ScanCode.KEY_J, VirtualKey.VK_J),
            ["Key_K"] = (ScanCode.KEY_K, VirtualKey.VK_K),
            ["Key_L"] = (ScanCode.KEY_L, VirtualKey.VK_L),
            ["Key_SemiColon"] = (ScanCode.OEM_1, VirtualKey.VK_OEM_1), // ; US
            ["Key_Apostrophe"] = (ScanCode.OEM_7, VirtualKey.VK_OEM_7), // ' US?
            ["Key_Grave"] = (ScanCode.OEM_3, VirtualKey.VK_OEM_3), // ` US?
            ["Key_LeftShift"] = (ScanCode.LSHIFT, VirtualKey.VK_LSHIFT),
            ["Key_BackSlash"] = (ScanCode.OEM_5, VirtualKey.VK_OEM_5), // \ US? or OEM_102?
            ["Key_Z"] = (ScanCode.KEY_Z, VirtualKey.VK_Z),
            ["Key_X"] = (ScanCode.KEY_X, VirtualKey.VK_X),
            ["Key_C"] = (ScanCode.KEY_C, VirtualKey.VK_C),
            ["Key_V"] = (ScanCode.KEY_V, VirtualKey.VK_V),
            ["Key_B"] = (ScanCode.KEY_B, VirtualKey.VK_B),
            ["Key_N"] = (ScanCode.KEY_N, VirtualKey.VK_N),
            ["Key_M"] = (ScanCode.KEY_M, VirtualKey.VK_M),
            ["Key_Comma"] = (ScanCode.OEM_COMMA, VirtualKey.VK_OEM_COMMA),
            ["Key_Period"] = (ScanCode.OEM_PERIOD, VirtualKey.VK_OEM_PERIOD),
            ["Key_Slash"] = (ScanCode.DIVIDE, VirtualKey.VK_DIVIDE),
            ["Key_RightShift"] = (ScanCode.RSHIFT, VirtualKey.VK_RSHIFT),
            ["Key_Numpad_Multiply"] = (ScanCode.MULTIPLY, VirtualKey.VK_MULTIPLY), // TODO: No numpad equivalent?
            ["Key_LeftAlt"] = (ScanCode.MENU, VirtualKey.VK_MENU),
            ["Key_Space"] = (ScanCode.SPACE, VirtualKey.VK_SPACE),
            ["Key_CapsLock"] = (ScanCode.CAPITAL, VirtualKey.VK_CAPITAL),
            ["Key_F1"] = (ScanCode.F1, VirtualKey.VK_F1),
            ["Key_F2"] = (ScanCode.F2, VirtualKey.VK_F2),
            ["Key_F3"] = (ScanCode.F3, VirtualKey.VK_F3),
            ["Key_F4"] = (ScanCode.F4, VirtualKey.VK_F4),
            ["Key_F5"] = (ScanCode.F5, VirtualKey.VK_F5),
            ["Key_F6"] = (ScanCode.F6, VirtualKey.VK_F6),
            ["Key_F7"] = (ScanCode.F7, VirtualKey.VK_F7),
            ["Key_F8"] = (ScanCode.F8, VirtualKey.VK_F8),
            ["Key_F9"] = (ScanCode.F9, VirtualKey.VK_F9),
            ["Key_F10"] = (ScanCode.F10, VirtualKey.VK_F10),
            ["Key_NumLock"] = (ScanCode.NUMLOCK, VirtualKey.VK_NUMLOCK),
            ["Key_ScrollLock"] = (ScanCode.SCROLL, VirtualKey.VK_SCROLL),
            ["Key_Numpad_7"] = (ScanCode.NUMPAD7, VirtualKey.VK_NUMPAD7),
            ["Key_Numpad_8"] = (ScanCode.NUMPAD8, VirtualKey.VK_NUMPAD8),
            ["Key_Numpad_9"] = (ScanCode.NUMPAD9, VirtualKey.VK_NUMPAD9),
            ["Key_Numpad_Subtract"] = (ScanCode.SUBTRACT, VirtualKey.VK_SUBTRACT), // TODO: No numpad equivalent?
            ["Key_Numpad_4"] = (ScanCode.NUMPAD4, VirtualKey.VK_NUMPAD4),
            ["Key_Numpad_5"] = (ScanCode.NUMPAD5, VirtualKey.VK_NUMPAD5),
            ["Key_Numpad_6"] = (ScanCode.NUMPAD6, VirtualKey.VK_NUMPAD6),
            ["Key_Numpad_Add"] = (ScanCode.ADD, VirtualKey.VK_ADD), // TODO: No numpad equivalent?
            ["Key_Numpad_1"] = (ScanCode.NUMPAD1, VirtualKey.VK_NUMPAD1),
            ["Key_Numpad_2"] = (ScanCode.NUMPAD2, VirtualKey.VK_NUMPAD2),
            ["Key_Numpad_3"] = (ScanCode.NUMPAD3, VirtualKey.VK_NUMPAD3),
            ["Key_Numpad_0"] = (ScanCode.NUMPAD0, VirtualKey.VK_NUMPAD0),
            ["Key_Numpad_Decimal"] = (ScanCode.DECIMAL, VirtualKey.VK_DECIMAL), // TODO: is this numpad only?
            ["Key_OEM_102"] = (ScanCode.OEM_102, VirtualKey.VK_OEM_102),
            ["Key_F11"] = (ScanCode.F11, VirtualKey.VK_F11),
            ["Key_F12"] = (ScanCode.F12, VirtualKey.VK_F12),
            ["Key_F13"] = (ScanCode.F13, VirtualKey.VK_F13),
            ["Key_F14"] = (ScanCode.F14, VirtualKey.VK_F14),
            ["Key_F15"] = (ScanCode.F15, VirtualKey.VK_F15),
            ["Key_Kana"] = (ScanCode.NONAME, VirtualKey.VK_KANA), // TODO: how to test?
            ["Key_ABNT_C1"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: how to test?
            ["Key_Convert"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: how to test?
            ["Key_NoConvert"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: how to test?
            ["Key_Yen"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: how to test?
            ["Key_ABNT_C2"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: how to test?
            ["Key_Numpad_Equals"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: how to test?
            ["Key_PrevTrack"] = (ScanCode.MEDIA_PREV_TRACK, VirtualKey.VK_MEDIA_PREV_TRACK),
            ["Key_AT"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: is this dupe of unmodified key?
            ["Key_Colon"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: is this dupe of unmodified key?
            ["Key_Underline"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: is this dupe of unmodified key?
            ["Key_Kanji"] = (ScanCode.NONAME, VirtualKey.VK_KANJI), // TODO: how to test?
            ["Key_Stop"] = (ScanCode.BROWSER_STOP, VirtualKey.VK_BROWSER_STOP), // TODO: is this browser?
            ["Key_AX"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: how to test?
            ["Key_Unlabeled"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: how to test?
            ["Key_NextTrack"] = (ScanCode.MEDIA_NEXT_TRACK, VirtualKey.VK_MEDIA_NEXT_TRACK),
            ["Key_Numpad_Enter"] = (ScanCode.RETURN, VirtualKey.VK_RETURN), // TODO: is this numpad only?
            ["Key_RightControl"] = (ScanCode.RCONTROL, VirtualKey.VK_RCONTROL),
            ["Key_Mute"] = (ScanCode.VOLUME_MUTE, VirtualKey.VK_VOLUME_MUTE),
            ["Key_Calculator"] = (ScanCode.NONAME, VirtualKey.VK_NONAME),
            ["Key_PlayPause"] = (ScanCode.NONAME, VirtualKey.VK_NONAME),
            ["Key_MediaStop"] = (ScanCode.NONAME, VirtualKey.VK_NONAME),
            ["Key_VolumeDown"] = (ScanCode.NONAME, VirtualKey.VK_NONAME),
            ["Key_VolumeUp"] = (ScanCode.NONAME, VirtualKey.VK_NONAME),
            ["Key_WebHome"] = (ScanCode.NONAME, VirtualKey.VK_NONAME),
            ["Key_Numpad_Comma"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: how to test?
            ["Key_Numpad_Divide"] = (ScanCode.DIVIDE, VirtualKey.VK_DIVIDE), // TODO: is this numpad only?
            ["Key_SYSRQ"] = (ScanCode.SNAPSHOT, VirtualKey.VK_SNAPSHOT), // SysReq/PrintScreen
            ["Key_RightAlt"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: ???
            ["Key_Pause"] = (ScanCode.NONAME, VirtualKey.VK_PAUSE), // TODO: ???
            ["Key_Home"] = (ScanCode.HOME, VirtualKey.VK_HOME),
            ["Key_UpArrow"] = (ScanCode.UP, VirtualKey.VK_UP),
            ["Key_PageUp"] = (ScanCode.PRIOR, VirtualKey.VK_PRIOR),
            ["Key_LeftArrow"] = (ScanCode.LEFT, VirtualKey.VK_LEFT),
            ["Key_RightArrow"] = (ScanCode.RIGHT, VirtualKey.VK_RIGHT),
            ["Key_End"] = (ScanCode.END, VirtualKey.VK_END),
            ["Key_DownArrow"] = (ScanCode.DOWN, VirtualKey.VK_DOWN),
            ["Key_PageDown"] = (ScanCode.NEXT, VirtualKey.VK_NEXT),
            ["Key_Insert"] = (ScanCode.INSERT, VirtualKey.VK_INSERT),
            ["Key_Delete"] = (ScanCode.DELETE, VirtualKey.VK_DELETE),
            ["Key_LeftWin"] = (ScanCode.LWIN, VirtualKey.VK_LWIN),
            ["Key_RightWin"] = (ScanCode.RWIN, VirtualKey.VK_RWIN),
            ["Key_Apps"] = (ScanCode.APPS, VirtualKey.VK_APPS),
            ["Key_Power"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: hope not!
            ["Key_Sleep"] = (ScanCode.SLEEP, VirtualKey.VK_SLEEP),
            ["Key_Wake"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: ???
            ["Key_WebSearch"] = (ScanCode.BROWSER_SEARCH, VirtualKey.VK_BROWSER_SEARCH),
            ["Key_WebFavourites"] = (ScanCode.BROWSER_FAVORITES, VirtualKey.VK_BROWSER_FAVORITES),
            ["Key_WebRefresh"] = (ScanCode.BROWSER_REFRESH, VirtualKey.VK_BROWSER_REFRESH),
            ["Key_WebStop"] = (ScanCode.BROWSER_STOP, VirtualKey.VK_BROWSER_STOP),
            ["Key_WebForward"] = (ScanCode.BROWSER_FORWARD, VirtualKey.VK_BROWSER_FORWARD),
            ["Key_WebBack"] = (ScanCode.BROWSER_BACK, VirtualKey.VK_BROWSER_BACK),
            ["Key_MyComputer"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: ???
            ["Key_Mail"] = (ScanCode.LAUNCH_MAIL, VirtualKey.VK_LAUNCH_MAIL),
            ["Key_MediaSelect"] = (ScanCode.LAUNCH_MEDIA_SELECT, VirtualKey.VK_LAUNCH_MEDIA_SELECT),
            ["Key_GreenModifier"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: ???
            ["Key_OrangeModifier"] = (ScanCode.NONAME, VirtualKey.VK_NONAME), // TODO: ???
        };

        public ScanCode GetScanCode(string keyName)
        {
            return Table[keyName].Item1;
        }

        public VirtualKey GetVirtualKey(string keyName)
        {
            return Table[keyName].Item2;
        }

    }
}
