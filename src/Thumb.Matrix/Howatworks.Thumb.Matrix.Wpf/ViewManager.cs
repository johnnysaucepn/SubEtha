using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Howatworks.Thumb.Matrix.Core;

namespace Howatworks.Thumb.Matrix.Wpf
{
    public static class ViewManager
    {
        public static MatrixApp App;
        public static HttpUploadClient Client;

        private static readonly Lazy<AuthenticationDialog> AuthenticationDialog = new Lazy<AuthenticationDialog>(() => new AuthenticationDialog());

        public static async Task<AuthenticationResult> Authenticate(string username, string password)
        {
            return await Client.Authenticate(username, password) ? AuthenticationResult.Success : AuthenticationResult.Failure;
        }

        public class AuthenticationResult
        {
            public bool Succeeded { get; private set; }

            public static AuthenticationResult Success => new AuthenticationResult { Succeeded = true };
            public static AuthenticationResult Failure => new AuthenticationResult { Succeeded = false };
        }

        public static void ShowAuthenticationDialog()
        {
            AuthenticationDialog.Value.Show();
        }

        public static void CloseAuthenticationDialog()
        {
            AuthenticationDialog.Value.Hide();
        }

        public static void ConfirmAuthenticationDialog()
        {
            AuthenticationDialog.Value.Hide();
        }
    }
}
