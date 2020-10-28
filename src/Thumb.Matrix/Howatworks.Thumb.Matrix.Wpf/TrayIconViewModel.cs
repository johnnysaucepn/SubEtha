using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Howatworks.Thumb.Matrix.Core;
using Howatworks.Thumb.Wpf;

namespace Howatworks.Thumb.Matrix.Wpf
{
    public class TrayIconViewModel
    {
        readonly MatrixApp _app;
        readonly AuthenticationDialog _authDialog;
        public TrayIconViewModel(MatrixApp app, AuthenticationDialog authDialog)
        {
            _app = app;
            _authDialog = authDialog;
        }

        public string NotCheckedText => Resources.NotifyIconNeverUpdatedLabel;

        public string LastCheckedText =>
            string.Format(
                Resources.NotifyIconLastUpdatedLabel.Replace("\\n", Environment.NewLine),
                _app.LastEntry.Value.LocalDateTime.ToString("g"),
                _app.LastChecked.Value.LocalDateTime.ToString("g")
                );

        public string DefaultText => Resources.NotifyIconDefaultLabel;

        public string PlainStatusDisplayText
        {
            get
            {
                if (_app.LastChecked.HasValue)
                {
                    if (_app.LastEntry.HasValue)
                        return LastCheckedText;
                    return NotCheckedText;
                }
                return DefaultText;
            }
        }

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
                    _authDialog.Show();
                }
            };
    }
}
