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
        public string NotCheckedText => Resources.NotifyIconNeverUpdatedLabel;

        public string LastCheckedText =>
            string.Format(
                Resources.NotifyIconLastUpdatedLabel.Replace("\\n", Environment.NewLine),
                ViewManager.App.LastEntry().Value.LocalDateTime.ToString("g"),
                ViewManager.App.LastChecked().Value.LocalDateTime.ToString("g")
                );

        public string DefaultText => Resources.NotifyIconDefaultLabel;

        public string PlainStatusDisplayText
        {
            get
            {
                if (ViewManager.App.LastChecked().HasValue)
                {
                    if (ViewManager.App.LastEntry().HasValue)
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
                    if (!ViewManager.App.IsAuthenticated)
                    {
                        ViewManager.ShowAuthenticationDialog();
                    }
                }
            };
    }
}
