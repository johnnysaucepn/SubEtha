using System;
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
            var config = new ThumbConfigBuilder("Matrix").Build();

            var logger = new Log4NetThumbLogging(config);
            logger.Configure();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ThumbCoreModule(config));
            builder.RegisterModule(new ThumbConsoleModule(config));
            builder.RegisterModule(new MatrixModule());
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<MatrixApp>();

                app.OnAuthenticationRequired += (sender, args) =>
                {
                    do
                    {
                        (string username, string password) = GetCredentials(app);

                        app.Authenticate(username, password);
                        if (!app.IsAuthenticated)
                        {
                            System.Console.WriteLine("Authentication failed!");
                        }
                    } while (!app.IsAuthenticated);
                };

                app.Initialize();
                System.Console.ReadKey();
            }
        }

        private static (string username, string password) GetCredentials(MatrixApp app)
        {
            string username;
            string password;
            System.Console.WriteLine("Authentication required");
            do
            {
                System.Console.Write("Username: ");

                username = System.Console.ReadLine();
                username = username?.Substring(0, Math.Min(username.Length, app.MaxUsernameLength));
            } while (string.IsNullOrWhiteSpace(username));
            do
            {
                System.Console.Write("Password: ");

                password = MaskedReadLine();
                password = password?.Substring(0, Math.Min(password.Length, app.MaxPasswordLength));
            } while (string.IsNullOrWhiteSpace(password));

            return (username, password);
        }

        private static string MaskedReadLine()
        {
            var input = string.Empty;

            ConsoleKeyInfo ch = System.Console.ReadKey(true);
            while (ch.Key != ConsoleKey.Enter)
            {
                input += ch.KeyChar;
                System.Console.Write('*');

                ch = System.Console.ReadKey(true);
            }
            System.Console.WriteLine();
            return input;
        }
    }
}
