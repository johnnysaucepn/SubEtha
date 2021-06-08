using System.IO;
using Xunit;

namespace Howatworks.SubEtha.Bindings.Test
{
    public class BindingSetReaderTests
    {
        private const string DefaultBindingsFilename = "dahkron.binds";
        private static readonly BindingSetReader Reader = new BindingSetReader(new FileInfo(DefaultBindingsFilename));

        [Fact]
        public void DeserializeRoot()
        {
            var bindings = Reader.Read();
            Assert.NotNull(bindings);

            Assert.Equal("Dahkron", bindings.PresetName);
            Assert.Equal(4, bindings.MajorVersion);
            Assert.Equal("en-US", bindings.KeyboardLayout);
        }

        [Fact]
        public void DeserializeAxis()
        {
            var bindings = Reader.Read();
            Assert.NotNull(bindings);

            Assert.NotNull(bindings.YawAxisRaw);
            Assert.Equal("Joy_ZAxis", bindings.YawAxisRaw.Binding.Key);
        }

        [Fact]
        public void DeserializeButton()
        {
            var bindings = Reader.Read();
            Assert.NotNull(bindings);

            Assert.NotNull(bindings.YawLeftButton);
            Assert.Equal("Key_Numpad_4", bindings.YawLeftButton.Primary.Key);

            Assert.Equal("", bindings.YawLeftButton.Secondary.Key);
        }

        [Fact]
        public void DeserializeModifiedButton()
        {
            var bindings = Reader.Read();
            Assert.NotNull(bindings);

            Assert.NotNull(bindings.LeftThrustButton);
            Assert.Equal("Key_Q", bindings.LeftThrustButton.Primary.Key);
            Assert.Empty(bindings.LeftThrustButton.Primary.Modifier);

            Assert.Equal("Pos_Joy_RXAxis", bindings.LeftThrustButton.Secondary.Key);
            Assert.Equal(2, bindings.LeftThrustButton.Secondary.Modifier.Count);
            Assert.Equal("Joy_1", bindings.LeftThrustButton.Secondary.Modifier[0].Key);
            Assert.Equal("Joy_7", bindings.LeftThrustButton.Secondary.Modifier[1].Key);
        }

        [Fact]
        public void DeserializeToggleButton()
        {
            var bindings = Reader.Read();

            Assert.NotNull(bindings.YawToRollButton.ToggleOn);
            Assert.False(bindings.YawToRollButton.ToggleOn.Value);

            Assert.NotNull(bindings.HeadLookToggle.ToggleOn);
            Assert.True(bindings.HeadLookToggle.ToggleOn.Value);
        }

        [Fact]
        public void DeserializeFloatOption()
        {
            var bindings = Reader.Read();

            Assert.Equal(0.40000001m, bindings.YawToRollSensitivity.Value, 8);
        }

        [Fact]
        public void DeserializeBoolOption()
        {
            var bindings = Reader.Read();

            Assert.True(bindings.EnableCameraLockOn.Value);
        }

        [Fact]
        public void DeserializeStringOption()
        {
            var bindings = Reader.Read();

            Assert.Equal("mute_toggle", bindings.MuteButtonMode.Value);
        }

        [Fact]
        public void DeserializeButtonHold()
        {
            var bindings = Reader.Read();

            Assert.NotNull(bindings.HumanoidItemWheelButton.Primary);
            Assert.NotNull(bindings.HumanoidItemWheelButton.Primary.Hold);
            Assert.False(bindings.HumanoidItemWheelButton.Primary.Hold.Value);

            Assert.NotNull(bindings.HumanoidZoomButton.Primary);
            Assert.NotNull(bindings.HumanoidZoomButton.Primary.Hold);
            Assert.True(bindings.HumanoidZoomButton.Primary.Hold.Value);
        }
    }
}
