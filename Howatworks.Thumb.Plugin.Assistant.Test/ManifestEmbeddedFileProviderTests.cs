using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace Howatworks.Thumb.Plugin.Assistant.Test
{
    public class ManifestEmbeddedFileProviderTests
    {
        private readonly ManifestEmbeddedFileProvider _provider;

        public ManifestEmbeddedFileProviderTests()
        {
            _provider = new ManifestEmbeddedFileProvider(
                Assembly.GetAssembly(typeof(Startup))
            );
        }

        [Fact]
        public void GetDirectoryContents_StaticContent_DirFound()
        {
            var dir = _provider.GetDirectoryContents("StaticContent");
            Assert.True(dir.Exists);
        }

        [Fact]
        public void GetDirectoryContents_StaticContent_DirNotFound()
        {
            var dir = _provider.GetDirectoryContents("MissingContent");
            Assert.False(dir.Exists);
        }

        [Fact]
        public void GetFileInfo_SocketTest_FileFound()
        {
            var file = _provider.GetFileInfo(@"StaticContent\index.html");
            Assert.True(file.Exists);
        }

        [Fact]
        public void GetFileInfo_SocketTest_FileNotFound()
        {
            var file = _provider.GetFileInfo(@"StaticContent\indexmissing.html");
            Assert.False(file.Exists);
        }

        [Fact]
        public void GetFileInfo_SocketTest_FileLoaded()
        {
            var file = _provider.GetFileInfo(@"StaticContent\index.html");
            var content = new StreamReader(file.CreateReadStream(), Encoding.UTF8).ReadToEnd();
            Assert.Contains("websocket.onmessage", content);
        }
    }
}
