using System;
using System.Threading;
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
                var client = scope.Resolve<HttpUploadClient>();

                var cts = new CancellationTokenSource();

                app.OnAuthenticationRequired += async (sender, args) =>
                {
                    var authenticated = false;
                    do
                    {
                        (string username, string password) = GetCredentials(client);

                        authenticated = await client.Authenticate(username, password);
                        if (!authenticated)
                        {
                            System.Console.WriteLine("Authentication failed!");
                        }
                    } while (!authenticated);
                };

                app.Run(cts.Token);
            }
        }

        private static (string username, string password) GetCredentials(HttpUploadClient client)
        {
            string username;
            string password;
            System.Console.WriteLine("Authentication required");
            do
            {
                System.Console.Write("Username: ");

                username = System.Console.ReadLine();
                username = username?.Substring(0, Math.Min(username.Length, client.MaxUsernameLength));
            } while (string.IsNullOrWhiteSpace(username));
            do
            {
                System.Console.Write("Password: ");

                password = MaskedReadLine();
                password = password?.Substring(0, Math.Min(password.Length, client.MaxPasswordLength));
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
