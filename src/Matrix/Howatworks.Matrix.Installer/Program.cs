using System;
using System.IO;
using System.Windows.Forms;
using WixSharp;

namespace Howatworks.Matrix.Installer
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
            var root = Path.GetFullPath(Path.Combine(currentFolder, @"..\..\..\..\..\"));
            var appRoot = Path.Combine(root, $@"Matrix\Howatworks.Matrix.Wpf\bin\{Configuration}\{TargetNetFramework}\");

            var project = new ManagedProject("SubEtha Matrix",
                new Dir(@"%ProgramFiles%\Howatworks\SubEtha Matrix",
                    new Files(appRoot + "*")
                ),
                new Dir("%StartMenu%",
                    new ExeFileShortcut("SubEtha Matrix", "[INSTALLDIR]Howatworks.Matrix.Wpf.exe", arguments: "")
                )
            )
            {
                Platform = Platform.x64,
                GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b"),
                ManagedUI = ManagedUI.Default,
                LicenceFile = "LICENSE.rtf",
                ControlPanelInfo = new ProductInfo()
                {
                    ProductIcon = "thumb_red.ico",
                    Manufacturer = "Howatworks"
                },
                Version = Version.Parse(GitVersionInformation.AssemblySemFileVer),
                OutFileName = Path.Combine(root, @"..\.build\Installers", "Howatworks.Matrix")
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