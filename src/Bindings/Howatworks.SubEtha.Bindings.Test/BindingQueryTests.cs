using System.IO;
using Howatworks.SubEtha.Bindings.Monitor;
using Xunit;

namespace Howatworks.SubEtha.Bindings.Test
{
    public class BindingQueryTests
    {
        private const string DefaultBindingsFilename = "dahkron.binds";
        private static readonly BindingSetReader Reader = new BindingSetReader(new FileInfo(DefaultBindingsFilename));

        [Fact]
        public void KeyBindingListForKeyboard()
        {
            var bindings = Reader.Read();
            Assert.NotNull(bindings);

            var bindingMapper = new BindingMapper(bindings);
            var foundBindings = bindingMapper.GetBoundButtons("Keyboard");

            Assert.Equal(128, foundBindings.Count);
        }

        [Fact]
        public void KeyBindingListForMouse()
        {
            var bindings = Reader.Read();
            Assert.NotNull(bindings);

            var bindingMapper = new BindingMapper(bindings);
            var foundBindings = bindingMapper.GetBoundButtons("Mouse");

            Assert.Equal(8, foundBindings.Count);
        }

        [Fact]
        public void KeyBindingListForKeyboardAndMouse()
        {
            var bindings = Reader.Read();
            Assert.NotNull(bindings);

            var bindingMapper = new BindingMapper(bindings);
            var foundBindings = bindingMapper.GetBoundButtons("Keyboard", "Mouse");

            Assert.Equal(136, foundBindings.Count);
        }
    }
}
