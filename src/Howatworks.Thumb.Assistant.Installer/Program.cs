using System;
using System.IO;
using System.Windows.Forms;
using WixSharp;

namespace Howatworks.Thumb.Assistant.Installer
{
    internal static class Program
    {
#if DEBUG
        private static string Configuration => "Debug";
#else
        private static string Configuration => "Release";
#endif
        private static string TargetNetFramework => "net472";

        private static void Main()
        {
            var currentFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory;
            var root = Path.GetFullPath(Path.Combine(currentFolder, @"..\..\..\..\"));
            var thumbTrayRoot = Path.Combine(root, $@"Howatworks.Thumb.Assistant\bin\{Configuration}\{TargetNetFramework}\");

            var project = new ManagedProject("SubEtha Thumb Assistant",
                new Dir(@"%ProgramFiles%\Howatworks\SubEtha Thumb Assistant",
                    new Files(thumbTrayRoot + "*")
                ),
                new Dir("%StartMenu%",
                    new ExeFileShortcut("SubEtha Thumb Assistant", "[INSTALLDIR]Howatworks.Thumb.Assistant.exe", arguments: "")
                )
            )
            {
                Platform = Platform.x64,
                GUID = new Guid("ff2b008b-9c7b-4965-bc02-7c72c66400cb"),
                ManagedUI = ManagedUI.Default,
                LicenceFile = "LICENSE.rtf",
                OutDir = "Artifacts",
                ControlPanelInfo = new ProductInfo()
                {
                    ProductIcon = "thumb_blue.ico",
                    Manufacturer = "Howatworks"
                },
                Version = Version.Parse(GitVersionInformation.AssemblySemFileVer),
                OutFileName = "SubEthaThumbAssistant"
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