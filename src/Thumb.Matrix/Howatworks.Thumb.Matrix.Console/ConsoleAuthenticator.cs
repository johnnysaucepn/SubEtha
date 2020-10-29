using Howatworks.Thumb.Matrix.Core;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howatworks.Thumb.Matrix.Console
{
    public class ConsoleAuthenticator : IMatrixAuthenticator
    {
        private readonly HttpUploadClient _client;

        public ConsoleAuthenticator(HttpUploadClient client)
        {
            _client = client;
        }

        public async Task<bool> RequestAuthentication()
        {
            (string username, string password) = ReadCredentials(_client);
            var authenticated = await _client.Authenticate(username, password);
            if (!authenticated)
            {
                System.Console.WriteLine("Authentication failed!");
            }
            return authenticated;
        }

        private static (string username, string password) ReadCredentials(HttpUploadClient client)
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
