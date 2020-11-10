using Howatworks.Thumb.Matrix.Core;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Howatworks.Thumb.Matrix.Wpf
{
    public class DialogAuthenticator : IMatrixAuthenticator
    {
        // TODO: This is not very pretty - we shouldn't need to poke around in the dialog internals like this
        private readonly AuthenticationDialog _dialog;
        private readonly AuthenticationDialogViewModel _vm;

        public DialogAuthenticator(AuthenticationDialog dialog, AuthenticationDialogViewModel vm)
        {
            _dialog = dialog;
            _vm = vm;
        }

        public async Task<bool> RequestAuthentication()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _dialog.Show();
            });
            // This is a little clunky - block the whole thread from continuing until the close event is triggered

            var result = await Observable.FromEventPattern<AuthenticationDialogEventArgs>(
                h => _vm.OnAuthenticationCompleted += h,
                h => _vm.OnAuthenticationCompleted -= h
            )
            .Select(x => x.EventArgs.AuthenticationSuccess)
            .FirstAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                _dialog.Hide();
            });

            return result;
        }
    }
}
