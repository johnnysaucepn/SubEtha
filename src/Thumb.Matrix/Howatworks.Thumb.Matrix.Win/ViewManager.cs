using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howatworks.Thumb.Matrix.Win
{
    public static class ViewManager
    {
        private static readonly Lazy<AuthenticationDialog> _authenticationDialog = new Lazy<AuthenticationDialog>(() => new AuthenticationDialog());

        public static void ShowAuthenticationDialog()
        {
            _authenticationDialog.Value.Show();
        }

        public static void CloseAuthenticationDialog()
        {
            _authenticationDialog.Value.Hide();
        }

        public static void ConfirmAuthenticationDialog()
        {
            _authenticationDialog.Value.Hide();
        }
    }
}
