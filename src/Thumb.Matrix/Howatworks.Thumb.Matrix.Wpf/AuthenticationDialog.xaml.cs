using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Howatworks.Thumb.Matrix.Wpf
{
    /// <summary>
    /// Interaction logic for AuthenticationDialog.xaml
    /// </summary>
    public partial class AuthenticationDialog : Window
    {
        public AuthenticationDialogViewModel ViewModel => (AuthenticationDialogViewModel)DataContext;

        public AuthenticationDialog()
        {
            InitializeComponent();
            Closing += (sender, evt) => evt.Cancel = true;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = ((PasswordBox)sender).Password;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ViewModel.CancelCommand.Execute(null);
        }
    }
}
