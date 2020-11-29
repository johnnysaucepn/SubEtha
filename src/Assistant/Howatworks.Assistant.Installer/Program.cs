using System;
using System.IO;
using System.Windows.Forms;
using WixSharp;

namespace Howatworks.Assistant.Installer
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
            var appRoot = Path.Combine(root, $@"Assistant\Howatworks.Assistant.Wpf\bin\{Configuration}\{TargetNetFramework}\");

            var project = new ManagedProject("SubEtha Assistant",
                new Dir(@"%ProgramFiles%\Howatworks\SubEtha Assistant",
                    new Files(appRoot + "*")
                ),
                new Dir("%StartMenu%",
                    new ExeFileShortcut("SubEtha Assistant", "[INSTALLDIR]Howatworks.Assistant.Wpf.exe", arguments: "")
                )
            )
            {
                Platform = Platform.x64,
                GUID = new Guid("ff2b008b-9c7b-4965-bc02-7c72c66400cb"),
                ManagedUI = ManagedUI.Default,
                LicenceFile = "LICENSE.rtf",
                ControlPanelInfo = new ProductInfo()
                {
                    ProductIcon = "thumb_blue.ico",
                    Manufacturer = "Howatworks"
                },
                Version = Version.Parse(GitVersionInformation.AssemblySemFileVer),
                OutFileName = "Howatworks.Assistant"
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