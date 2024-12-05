using OOPA2.Database;
using OOPA2.Enums;
using OOPA2.Helpers;
using OOPA2.Models;

namespace OOPA2
{
    // Class for the Create User Form
    public partial class FrmCreateUser : Form
    {
        private readonly DatabaseController _dbController = new();

        private bool ShowPassword = false;
        private readonly char NullCharacter = '\0';

        // Constants for the Open Eye and Locked Padlock symbols
        private readonly string OpenEye = "👁";
        private readonly string LockedPadlock = "🔒";

        public FrmCreateUser()
        {
            /*
            Constructor for FrmCreateUser
            */
            InitializeComponent();
        }

        private void btnShowPassword_Click(object sender, EventArgs e)
        {
            /*
            Method to toggle the password visibility on button click
            */

            this.ShowPassword = !this.ShowPassword;

            char passwordChar = this.ShowPassword ? this.NullCharacter : '*';

            txtPassword.PasswordChar = passwordChar;
            txtPasswordConfirm.PasswordChar = passwordChar;

            string buttonSymbol = this.ShowPassword ? this.LockedPadlock : this.OpenEye;

            btnShowPassword.Text = buttonSymbol;
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            /*
            Method to create the user on button click
            */
            
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string passwordConfirm = txtPasswordConfirm.Text;

            ResponseModel isValidUsernameResponse = UsersHelper.IsValidUsername(username);

            if (!isValidUsernameResponse.IsSuccess)
            {
                MessageBox.Show($"Invalid Username - {isValidUsernameResponse.Reason}");
                return;
            }

            if (password != passwordConfirm)
            {
                MessageBox.Show("Invalid Password - Passwords should match");
                return;
            }

            ResponseModel isValidPasswordResponse = UsersHelper.IsValidPassword(password);

            if (!isValidPasswordResponse.IsSuccess)
            {
                MessageBox.Show($"Invalid Password - {isValidPasswordResponse.Reason}");
                return;
            }

            this._dbController.AddUser(username, password, UserRolesEnum.Admin);

            MessageBox.Show("User Created");
            this.Close();
        }
    }
}
