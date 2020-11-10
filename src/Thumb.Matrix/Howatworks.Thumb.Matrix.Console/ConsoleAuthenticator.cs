using Howatworks.Thumb.Console;
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
        private readonly ConsoleKeyListener _keyListener;

        public ConsoleAuthenticator(HttpUploadClient client, ConsoleKeyListener keyListener)
        {
            _client = client;
            _keyListener = keyListener;
        }

        public async Task<bool> RequestAuthentication()
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            (string username, string password) = await ReadCredentials(_client);
            var authenticated = await _client.Authenticate(username, password);
            if (!authenticated)
            {
                System.Console.WriteLine("Authentication failed!");
            }
            return authenticated;
        }

        private async Task<(string username, string password)> ReadCredentials(HttpUploadClient client)
        {
            string username;
            string password;
            System.Console.WriteLine("Authentication required");
            do
            {
                System.Console.Write("Username: ");

                username = await ClearReadLine();
                username = username?.Substring(0, Math.Min(username.Length, client.MaxUsernameLength));
            } while (string.IsNullOrWhiteSpace(username));
            do
            {
                System.Console.Write("Password: ");

                password = await MaskedReadLine();
                password = password?.Substring(0, Math.Min(password.Length, client.MaxPasswordLength));
            } while (string.IsNullOrWhiteSpace(password));

            return (username, password);
        }

        private async Task<string> ClearReadLine()
        {
            return await ReadLine(c => c);
        }

        private async Task<string> MaskedReadLine()
        {
            return await ReadLine(c => '*');
        }

        private async Task<string> ReadLine(Func<char, char> maskingFunc)
        { 
            var sb = new StringBuilder();

            var start = Observable.Start(() =>
            {
            });

            var keyObs = _keyListener.Observable.TakeWhile(x => x.Key != ConsoleKey.Enter)
                .Aggregate(new List<char>(), (a, k) =>
                {
                    if (k.Key == ConsoleKey.Backspace)
                    {
                        if (a.Count > 0)
                        {
                            a.RemoveAt(a.Count - 1);
                            System.Console.CursorLeft--;
                            System.Console.Write(' ');
                            System.Console.CursorLeft--;
                        }
                    }
                    else if (k.KeyChar != 0)
                    {
                        a.Add(k.KeyChar);
                        System.Console.Write(maskingFunc(k.KeyChar));
                    }
                    return a;
                });
            var chars = await keyObs;

            System.Console.WriteLine();

            return new string(chars.ToArray());
        }

    }
}
