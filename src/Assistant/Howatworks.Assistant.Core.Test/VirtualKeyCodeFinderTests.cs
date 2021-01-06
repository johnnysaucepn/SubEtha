using Howatworks.Assistant.Core.ControlSimulators;
using InputSimulatorStandard.Native;
using System.Globalization;
using Xunit;

namespace Howatworks.Assistant.Core.Test
{
    public class VirtualKeyCodeFinderTests
    {
        private readonly CultureInfo us = CultureInfo.GetCultureInfo("en-US");
        private readonly CultureInfo gb = CultureInfo.GetCultureInfo("en-GB");

        public class InvariantTestData : TheoryData<char, VirtualKeyCode>
        {
            public InvariantTestData()
            {
                Add('a', VirtualKeyCode.VK_A);
                Add('B', VirtualKeyCode.VK_B);
                Add('0', VirtualKeyCode.VK_0);
                Add(',', VirtualKeyCode.OEM_COMMA);
                Add('*', VirtualKeyCode.VK_8);
            }
        }

        public class USTestData : TheoryData<string, VirtualKeyCode>
        {
            public USTestData()
            {
                Add(":", VirtualKeyCode.OEM_1);
                Add("'", VirtualKeyCode.OEM_7);
                Add("@", VirtualKeyCode.VK_2);
                Add("#", VirtualKeyCode.VK_3);
            }
        }

        public class GBTestData : TheoryData<string, VirtualKeyCode>
        {
            public GBTestData()
            {
                Add(":", VirtualKeyCode.OEM_1);
                Add("'", VirtualKeyCode.OEM_3);
                Add("@", VirtualKeyCode.OEM_3); // On GB keyboards, these are the same key
                Add("£", VirtualKeyCode.VK_3);
                Add("#", VirtualKeyCode.OEM_7);
            }
        }

        [Theory]
        [ClassData(typeof(InvariantTestData))]
        public void TryGetKey_InvariantCulture_ReturnsVKCode(char character, VirtualKeyCode vkCode)
        {
            var finder = new VirtualKeyCodeFinder(CultureInfo.InvariantCulture);

            var found = finder.TryGetKey(character, out var foundVirtualKeyCode);
            Assert.True(found);
            Assert.Equal(vkCode, foundVirtualKeyCode);
        }

        [Theory]
        [ClassData(typeof(USTestData))]
        public void TryGetKey_USCulture_ReturnsVKCode(char character, VirtualKeyCode vkCode)
        {
            var finder = new VirtualKeyCodeFinder(us);

            var found = finder.TryGetKey(character, out var foundVirtualKeyCode);
            Assert.True(found);
            Assert.Equal(vkCode, foundVirtualKeyCode);
        }

        [Theory]
        [ClassData(typeof(GBTestData))]
        public void TryGetKey_GBCulture_ReturnsVKCode(char character, VirtualKeyCode vkCode)
        {
            var finder = new VirtualKeyCodeFinder(gb);

            var found = finder.TryGetKey(character, out var foundVirtualKeyCode);
            Assert.True(found);
            Assert.Equal(vkCode, foundVirtualKeyCode);
        }

        [Fact]
        public void TryGetKey_USCulture_PoundSterlingNotFound()
        {
            var finder = new VirtualKeyCodeFinder(us);

            var found = finder.TryGetKey('£', out var _);
            Assert.False(found);
        }

        [Fact]
        public void GetKeyOrDefault_KeyFound_ReturnsVKCode()
        {
            var finder = new VirtualKeyCodeFinder(us);

            var found = finder.GetKeyOrDefault('&', VirtualKeyCode.NONAME);
            Assert.NotEqual(VirtualKeyCode.NONAME, found);
        }

        [Fact]
        public void GetKeyOrDefault_KeyNotFound_ReturnsDefaultVKCode()
        {
            var finder = new VirtualKeyCodeFinder(us);

            var found = finder.GetKeyOrDefault('£', VirtualKeyCode.NONAME);
            Assert.Equal(VirtualKeyCode.NONAME, found);
        }
    }
}
