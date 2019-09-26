using System;
using System.Windows.Forms;

namespace Howatworks.Thumb.Matrix
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public event EventHandler<LoginEventArgs> OnLogin = delegate { };

        public string SiteName
        {
            set => SiteLabel.Text = value;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Icon = Resources.ThumbIcon;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            OnLogin(this, new LoginEventArgs(UsernameBox.Text, PasswordBox.Text));
        }
    }
}
