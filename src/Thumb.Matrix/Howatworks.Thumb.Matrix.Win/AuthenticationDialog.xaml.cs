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

namespace Howatworks.Thumb.Matrix.Win
{
    /// <summary>
    /// Interaction logic for AuthenticationDialog.xaml
    /// </summary>
    public partial class AuthenticationDialog : Window
    {
        public AuthenticationDialog()
        {
            InitializeComponent();
            Closing += (sender, evt) => evt.Cancel = true;
        }
    }
}
