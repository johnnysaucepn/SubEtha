using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Howatworks.Thumb.Matrix.Core;
using Howatworks.Thumb.Wpf;

namespace Howatworks.Thumb.Matrix.Wpf
{
    public class TrayIconViewModel /*: INotifyPropertyChanged*/
    {
        public MatrixApp App { get; private set; }

        public static TrayIconViewModel Create(MatrixApp app) => new TrayIconViewModel() { App = app };

        public event EventHandler OnExitApplication = delegate { };
        public event EventHandler OnAuthenticationRequested = delegate { };

        public string NotCheckedText => Resources.NotifyIconNeverUpdatedLabel;

        public string LastCheckedText =>
            string.Format(
                Resources.NotifyIconLastUpdatedLabel.Replace("\\n", Environment.NewLine),
                App?.LastEntry.Value.LocalDateTime.ToString("g"),
                App?.LastChecked.Value.LocalDateTime.ToString("g")
                );

        public string DefaultText => Resources.NotifyIconDefaultLabel;

        public string PlainStatusDisplayText
        {
            get
            {
                if (App?.LastChecked != null)
                {
                    if (App?.LastEntry != null)
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
                CommandAction = () => OnExitApplication(this, EventArgs.Empty)
            };

        public ICommand ShowAuthDialogCommand =>
            new DelegateCommand
            {
                CommandAction = () => OnAuthenticationRequested(this, EventArgs.Empty)
            };

        /*public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/
    }
}
