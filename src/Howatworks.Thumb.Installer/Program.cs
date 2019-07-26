using System;
using System.IO;
using System.Windows.Forms;
using WixSharp;

namespace Howatworks.Thumb.Installer
{
    internal static class Program
    {
#if DEBUG
        private static string Configuration => "Debug";
#else
        private static readonly string Configuration => "Release";
#endif
        private static string TargetNetFramework => "net472";

        private static void Main()
        {
            var currentFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory;
            var root = Path.GetFullPath(Path.Combine(currentFolder, @"..\..\..\..\"));
            var thumbTrayRoot = Path.Combine(root, $@"Howatworks.Thumb.Tray\bin\{Configuration}\{TargetNetFramework}\");

            var project = new ManagedProject("SubEtha Thumb",
                new Dir(@"%ProgramFiles%\Howatworks\SubEtha Thumb",
                    new Files(thumbTrayRoot + "*")
                ),
                new Dir(@"%Desktop%",
                    new ExeFileShortcut("Thumb", "[INSTALL_DIR]Howatworks.Thumb.Tray.exe", arguments: "")
                )
            )
            {
                Platform = Platform.x64,
                GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b"),
                ManagedUI = ManagedUI.Default,
                LicenceFile = Path.Combine(thumbTrayRoot, "LICENSE"),
                OutDir = "Artifacts",
                ControlPanelInfo = new ProductInfo()
                {
                    Manufacturer = "Howatworks"
                },
                Version = Version.Parse(GitVersionInformation.AssemblySemFileVer),
                OutFileName = "SubEthaThumb"
            };

            project.Load += Msi_Load;
            project.BeforeInstall += Msi_BeforeInstall;
            project.AfterInstall += Msi_AfterInstall;

            Compiler.AllowNonRtfLicense = true;
            project.BuildMsi();
        }

        private static void Msi_Load(SetupEventArgs e)
        {
            if (!e.IsUISupressed && !e.IsUninstalling)
                MessageBox.Show(e.ToString(), "Load");
        }

        private static void Msi_BeforeInstall(SetupEventArgs e)
        {
            if (!e.IsUISupressed && !e.IsUninstalling)
                MessageBox.Show(e.ToString(), "BeforeInstall");
        }

        private static void Msi_AfterInstall(SetupEventArgs e)
        {
            if (!e.IsUISupressed && !e.IsUninstalling)
                MessageBox.Show(e.ToString(), "AfterExecute");
        }
    }
}