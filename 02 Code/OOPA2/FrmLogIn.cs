using OOPA2.Database;
using OOPA2.Helpers;
using OOPA2.Models;

namespace OOPA2
{
    // Class for the Log In Form
    public partial class FrmLogIn : Form
    {
        private readonly DatabaseController _dbController = new();
        private bool ShowPassword = false;
        private readonly char NullCharacter = '\0';

        // Constants for the Open Eye and Locked Padlock symbols
        private readonly string OpenEye = "👁";
        private readonly string LockedPadlock = "🔒";

        public FrmLogIn()
        {
            /*
            Constructor for FrmLogIn
            */

            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            /*
            Method to log in the user on button click
            */

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            ResponseModel validCredentialsResponse = _dbController.ValidateUser(username, password);

            if (!validCredentialsResponse.IsSuccess)
            {
                MessageBox.Show($"Invalid Credentials - {validCredentialsResponse.Reason}");
                return;
            }

            UsersHelper.LoggedInUsername = username;

            FrmMain frmMain = new();
            frmMain.Show();
            this.Hide();

        }

        private void btnShowPassword_Click(object sender, EventArgs e)
        {
            /*
            Method to toggle the password visibility on button click
            */
            
            this.ShowPassword = !this.ShowPassword;

            char passwordChar = this.ShowPassword ? this.NullCharacter : '*';

            txtPassword.PasswordChar = passwordChar;

            string buttonSymbol = this.ShowPassword ? this.LockedPadlock : this.OpenEye;

            btnShowPassword.Text = buttonSymbol;
        }
    }
}
