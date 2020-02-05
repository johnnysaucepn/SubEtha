using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Howatworks.Thumb.Matrix.Win
{
    public class TrayIconViewModel
    {
        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand =>
            new DelegateCommand
            {
                CommandAction = () => Application.Current.Shutdown()
            };

        public ICommand ShowAuthDialogCommand =>
            new DelegateCommand
            {
                CommandAction = () =>
                {
                    ViewManager.ShowAuthenticationDialog();
                }
            };
    }
}
