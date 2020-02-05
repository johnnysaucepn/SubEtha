using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Howatworks.Thumb.Matrix.Win
{
    public class AuthenticationDialogViewModel
    {
        public ICommand CancelCommand =>
            new DelegateCommand
            {
                CommandAction = () => ViewManager.CloseAuthenticationDialog()
            };

        public ICommand OkCommand =>
            new DelegateCommand
            {
                CommandAction = () => ViewManager.ConfirmAuthenticationDialog()
            };
    }
}
