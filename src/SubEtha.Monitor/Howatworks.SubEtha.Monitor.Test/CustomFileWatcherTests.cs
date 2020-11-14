using System;
using System.IO;
using System.Threading;
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

        private void WaitForNotification()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(150));
        }

        [Fact]
        public void CreatedFiles_MatchingFileCreated_Found()
        {
            var found = false;
            var expectedFilePath = Path.Combine(TestDir.FullName, "created.txt");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.CreatedFiles.Subscribe(f =>
            {
                if (f.Equals(expectedFilePath)) found = true;
            });
            watcher.Start();
            File.WriteAllText(expectedFilePath, "dummy text");
            WaitForNotification();
            watcher.Stop();
            Assert.True(found);
        }

        [Fact]
        public void CreatedFiles_NonMatchingFileCreated_NotFound()
        {
            var found = false;
            var expectedFilePath = Path.Combine(TestDir.FullName, "created.bad");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.CreatedFiles.Subscribe(f =>
            {
                if (f.Equals(expectedFilePath)) found = true;
            });
            watcher.Start();
            File.WriteAllText(expectedFilePath, "dummy text");
            WaitForNotification();
            watcher.Stop();
            Assert.False(found);
        }

        [Fact]
        public void CreatedFiles_MatchingFileRenamed_Found()
        {
            var found = false;
            var originalFilePath = Path.Combine(TestDir.FullName, "renamed.bad");
            File.WriteAllText(originalFilePath, "dummy text");
            var newFilePath = Path.Combine(TestDir.FullName, "renamed.txt");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.CreatedFiles.Subscribe(f =>
            {
                if (f.Equals(newFilePath)) found = true;
            });
            watcher.Start();
            File.Move(originalFilePath, newFilePath);
            WaitForNotification();
            watcher.Stop();
            Assert.True(found);
        }

        [Fact]
        public void CreatedFiles_NonMatchingFileRenamed_NotFound()
        {
            var found = false;
            var originalFilePath = Path.Combine(TestDir.FullName, "renamed.txt");
            File.WriteAllText(originalFilePath, "dummy text");
            var newFilePath = Path.Combine(TestDir.FullName, "renamed.bad");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.CreatedFiles.Subscribe(f =>
            {
                if (f.Equals(newFilePath)) found = true;
            });
            watcher.Start();
            File.Move(originalFilePath, newFilePath);
            WaitForNotification();
            watcher.Stop();
            Assert.False(found);
        }

        [Fact]
        public void DeletedFiles_MatchingFileDeleted_Found()
        {
            var found = false;
            var expectedFilePath = Path.Combine(TestDir.FullName, "deleted.txt");
            File.WriteAllText(expectedFilePath, "dummy text");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.DeletedFiles.Subscribe(f =>
            {
                if (f.Equals(expectedFilePath)) found = true;
            });
            watcher.Start();
            File.Delete(expectedFilePath);
            WaitForNotification();
            watcher.Stop();
            Assert.True(found);
        }

        [Fact]
        public void DeletedFiles_NonMatchingFileDeleted_NotFound()
        {
            var found = false;
            var expectedFilePath = Path.Combine(TestDir.FullName, "deleted.bad");
            File.WriteAllText(expectedFilePath, "dummy text");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.DeletedFiles.Subscribe(f =>
            {
                if (f.Equals(expectedFilePath)) found = true;
            });
            watcher.Start();
            File.Delete(expectedFilePath);
            WaitForNotification();
            watcher.Stop();
            Assert.False(found);
        }

        [Fact]
        public void DeletedFiles_MatchingFileRenamed_Found()
        {
            var found = false;
            var originalFilePath = Path.Combine(TestDir.FullName, "renamed.txt");
            File.WriteAllText(originalFilePath, "dummy text");
            var newFilePath = Path.Combine(TestDir.FullName, "renamed.bad");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.DeletedFiles.Subscribe(f =>
            {
                if (f.Equals(originalFilePath)) found = true;
            });
            watcher.Start();
            File.Move(originalFilePath, newFilePath);
            WaitForNotification();
            watcher.Stop();
            Assert.True(found);
        }

        [Fact]
        public void DeletedFiles_NonMatchingFileRenamed_NotFound()
        {
            var found = false;
            var originalFilePath = Path.Combine(TestDir.FullName, "renamed.bad");
            File.WriteAllText(originalFilePath, "dummy text");
            var newFilePath = Path.Combine(TestDir.FullName, "renamed.txt");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.DeletedFiles.Subscribe(f =>
            {
                if (f.Equals(originalFilePath)) found = true;
            });
            watcher.Start();
            File.Move(originalFilePath, newFilePath);
            WaitForNotification();
            watcher.Stop();
            Assert.False(found);
        }

        [Fact]
        public void ChangedFiles_MatchingFileChanged_Found()
        {
            var found = false;
            var expectedFilePath = Path.Combine(TestDir.FullName, "changed.txt");
            File.WriteAllText(expectedFilePath, "dummy text");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.ChangedFiles.Subscribe(f =>
            {
                if (f.Equals(expectedFilePath)) found = true;
            });
            watcher.Start();
            File.WriteAllText(expectedFilePath, "other text");
            WaitForNotification();
            watcher.Stop();
            Assert.True(found);
        }

        [Fact]
        public void ChangedFiles_NonMatchingFileChanged_NotFound()
        {
            var found = false;
            var expectedFilePath = Path.Combine(TestDir.FullName, "changed.bad");
            File.WriteAllText(expectedFilePath, "dummy text");
            var watcher = new CustomFileWatcher(TestDir.FullName, "*.txt");
            watcher.DeletedFiles.Subscribe(f =>
            {
                if (f.Equals(expectedFilePath)) found = true;
            });
            watcher.Start();
            File.WriteAllText(expectedFilePath, "other text");
            WaitForNotification();
            watcher.Stop();
            Assert.False(found);
        }
    }
}
