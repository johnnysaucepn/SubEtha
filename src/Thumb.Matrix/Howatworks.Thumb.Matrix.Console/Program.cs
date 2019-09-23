using System;
using System.IO;
using Autofac;
using Howatworks.Thumb.Console;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Matrix.Core;

namespace Howatworks.Thumb.Matrix.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var appStoragePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Howatworks", "Thumb", "Matrix"
            );
            var config = new ThumbConfigBuilder(appStoragePath).Build();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbConsoleModule(config));
            builder.RegisterModule(new MatrixModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<MatrixApp>();
                
                app.OnAuthenticationRequired += (sender, args) => {
                    app.Stop();
                    do
                    {
                        string username;
                        string password;
                        System.Console.WriteLine("Authentication required");
                        do
                        {
                            System.Console.WriteLine("Username:");

                            username = System.Console.ReadLine();
                            username = username.Substring(0, Math.Min(username.Length, app.MaxUsernameLength));
                        } while (string.IsNullOrWhiteSpace(username));
                        do
                        {
                            System.Console.WriteLine("Password:");

                            password = System.Console.ReadLine();
                            password = password.Substring(0, Math.Min(password.Length, app.MaxPasswordLength));
                        } while (string.IsNullOrWhiteSpace(password));

                        app.Authenticate(username, password);
                        if (!app.IsAuthenticated)
                        {
                            System.Console.WriteLine("Authentication failed!");
                        }
                    } while (app.IsAuthenticated);
                };

                app.Initialize();

                app.Start();
                System.Console.ReadKey();
                app.Stop();
            }
        }

    }
}
