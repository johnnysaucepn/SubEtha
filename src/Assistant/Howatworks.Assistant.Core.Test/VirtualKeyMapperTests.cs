using Howatworks.Assistant.Core.ControlSimulators;
using InputSimulatorStandard.Native;
using System.Globalization;
using Xunit;

namespace Howatworks.Assistant.Core.Test
{

    public class VirtualKeyMapperTests
    {
        private readonly CultureInfo us = CultureInfo.GetCultureInfo("en-US");
        private readonly CultureInfo gb = CultureInfo.GetCultureInfo("en-GB");
        private readonly CultureInfo ru = CultureInfo.GetCultureInfo("ru");

        public class SimpleTestData : TheoryData<string, VirtualKeyCode>
        {
            public SimpleTestData()
            {
                Add("Key_A", VirtualKeyCode.VK_A);
                Add("Key_B", VirtualKeyCode.VK_B);
                Add("Key_0", VirtualKeyCode.VK_0);
                Add("Key_Escape", VirtualKeyCode.ESCAPE);
                Add("Key_Tab", VirtualKeyCode.TAB);
                Add("Key_LeftShift", VirtualKeyCode.LSHIFT);
                Add("Key_RightControl", VirtualKeyCode.RCONTROL);
            }
        }

        public class USTestData : TheoryData<string, VirtualKeyCode>
        {
            public USTestData()
            {
                Add("Key_Colon", VirtualKeyCode.OEM_1);
                Add("Key_Comma", VirtualKeyCode.OEM_COMMA);
                Add("Key_Apostrophe", VirtualKeyCode.OEM_7);
                Add("Key_LeftBracket", VirtualKeyCode.OEM_4);
                Add("Key_Minus", VirtualKeyCode.OEM_MINUS);
                Add("Key_Numpad_Decimal", VirtualKeyCode.DECIMAL);
            }
        }

        public class GBTestData : TheoryData<string, VirtualKeyCode>
        {
            public GBTestData()
            {
                Add("Key_Colon", VirtualKeyCode.OEM_1);
                Add("Key_Comma", VirtualKeyCode.OEM_COMMA);
                Add("Key_Apostrophe", VirtualKeyCode.OEM_3);
                Add("Key_LeftBracket", VirtualKeyCode.OEM_4);
                Add("Key_Minus", VirtualKeyCode.OEM_MINUS);
                Add("Key_Numpad_Decimal", VirtualKeyCode.DECIMAL);
            }
        }

        public class RUTestData : TheoryData<string, VirtualKeyCode>
        {
            public RUTestData()
            {
                Add("Key_Colon", VirtualKeyCode.OEM_1);
                Add("Key_Comma", VirtualKeyCode.OEM_COMMA);
                Add("Key_Apostrophe", VirtualKeyCode.OEM_7);
                Add("Key_LeftBracket", VirtualKeyCode.OEM_4);
                Add("Key_Minus", VirtualKeyCode.OEM_MINUS);
                Add("Key_Numpad_Decimal", VirtualKeyCode.DECIMAL);
            }
        }

        [Theory]
        [ClassData(typeof(SimpleTestData))]
        public void GetKey_AllCultures_ReturnsVKCode(string keyName, VirtualKeyCode vkCode)
        {
            var mapper = new VirtualKeyMapper();

            Assert.Equal(vkCode, mapper.MapKey(keyName));
        }

        [Theory]
        [ClassData(typeof(USTestData))]
        public void GetKey_USCulture_ReturnsVKCode(string keyName, VirtualKeyCode vkCode)
        {
            var mapper = new VirtualKeyMapper(us);

            Assert.Equal(vkCode, mapper.MapKey(keyName));
        }

        [Theory]
        [ClassData(typeof(GBTestData))]
        public void GetKey_GBCulture_ReturnsVKCode(string keyName, VirtualKeyCode vkCode)
        {
            var mapper = new VirtualKeyMapper(gb);

            Assert.Equal(vkCode, mapper.MapKey(keyName));
        }

        [Theory]
        [ClassData(typeof(RUTestData))]
        public void GetKey_RUCulture_ReturnsVKCode(string keyName, VirtualKeyCode vkCode)
        {
            var mapper = new VirtualKeyMapper(ru);

            Assert.Equal(vkCode, mapper.MapKey(keyName));
        }

        [Fact]
        public void MapKeys_GBCulture_ReturnsVKCodeList()
        {
            var mapper = new VirtualKeyMapper(gb);

            var expectedList = new VirtualKeyCode[] { VirtualKeyCode.OEM_1, VirtualKeyCode.OEM_COMMA, VirtualKeyCode.OEM_3, VirtualKeyCode.OEM_4, VirtualKeyCode.OEM_MINUS, VirtualKeyCode.DECIMAL };
            var vkList = mapper.MapKeys("Key_Colon", "Key_Comma", "Key_Apostrophe", "Key_LeftBracket", "Key_Minus", "Key_Numpad_Decimal");

            Assert.Equal(expectedList, vkList);
        }
    }
}
