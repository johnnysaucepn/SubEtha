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

namespace Howatworks.Matrix.Wpf
{
    /// <summary>
    /// Interaction logic for AuthenticationDialog.xaml
    /// </summary>
    public partial class AuthenticationDialog : Window
    {
        readonly AuthenticationDialogViewModel _viewModel;
        
        public AuthenticationDialog(AuthenticationDialogViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            Closing += AuthenticationDialog_Closing;
            _viewModel.OnCloseRequested += (s, e) => { Hide(); };
            InitializeComponent();
        }

        private void AuthenticationDialog_Closing(object sender, CancelEventArgs e)
        {
            // Don't actually close the dialog - we're also binding the dialog close event to the viewmodel CloseCommand so we can handle consistently
            e.Cancel = true;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = ((PasswordBox)sender).Password;
        }

    }
}
