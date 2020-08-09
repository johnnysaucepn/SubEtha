using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Howatworks.SubEtha.Monitor.Test
{
    public class CustomFileWatcherTests : IDisposable
    {
        private readonly DirectoryInfo TestDir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "Howatworks", "CustomFileWatcherTests"));

        public CustomFileWatcherTests()
        {
            TestDir.Create();
            foreach (var file in TestDir.GetFiles()) { file.Delete(); }
        }
      
        public void Dispose()
        {
            foreach (var file in TestDir.GetFiles()) { file.Delete(); }
            TestDir.Delete();
        }

        [Fact]
        private void CreatedFiles_MatchingFileCreated_Found()
        {
            var found = false;
            var expectedFilePath = Path.Combine(TestDir.FullName, "created.txt");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.CreatedFiles.Subscribe(f =>
            {
                if (f.Equals(expectedFilePath))
                    found = true;
            }

            );
            watcher.Start();
            File.WriteAllText(expectedFilePath, "dummy text");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            watcher.Stop();
            Assert.True(found);
        }

        [Fact]
        private void CreatedFiles_NonMatchingFileCreated_NotFound()
        {
            var found = false;
            var expectedFilePath = Path.Combine(TestDir.FullName, "created.bad");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.CreatedFiles.Subscribe(f =>
            {
                if (f.Equals(expectedFilePath))
                    found = true;
            }

            );
            watcher.Start();
            File.WriteAllText(expectedFilePath, "dummy text");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            watcher.Stop();
            Assert.False(found);
        }
    }
}
