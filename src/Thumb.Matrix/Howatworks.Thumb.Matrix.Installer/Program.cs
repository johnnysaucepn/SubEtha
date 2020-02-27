using System;
using System.IO;
using System.Windows.Forms;
using WixSharp;

namespace Howatworks.Thumb.Matrix.Installer
{
    internal static class Program
    {
#if DEBUG
        private static string Configuration => "Debug";
#else
        private static string Configuration => "Release";
#endif
        private static string TargetNetFramework => "netcoreapp3.1";

        private static void Main()
        {
            var currentFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory;
            var root = Path.GetFullPath(Path.Combine(currentFolder, @"..\..\..\..\"));
            var thumbTrayRoot = Path.Combine(root, $@"Howatworks.Thumb.Matrix.Win\bin\{Configuration}\{TargetNetFramework}\");

            var project = new ManagedProject("SubEtha Thumb Matrix",
                new Dir(@"%ProgramFiles%\Howatworks\SubEtha Thumb Matrix",
                    new Files(thumbTrayRoot + "*")
                ),
                new Dir("%StartMenu%",
                    new ExeFileShortcut("SubEtha Thumb Matrix", "[INSTALLDIR]Howatworks.Thumb.Matrix.Win.exe", arguments: "")
                )
            )
            {
                Platform = Platform.x64,
                GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b"),
                ManagedUI = ManagedUI.Default,
                LicenceFile = "LICENSE.rtf",
                OutDir = "Artifacts",
                ControlPanelInfo = new ProductInfo()
                {
                    ProductIcon = "thumb_red.ico",
                    Manufacturer = "Howatworks"
                },
                Version = Version.Parse(GitVersionInformation.AssemblySemFileVer),
                OutFileName = "SubEthaThumbMatrix"
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