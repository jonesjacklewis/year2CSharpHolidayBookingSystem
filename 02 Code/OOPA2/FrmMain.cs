using OOPA2.Helpers;

namespace OOPA2
{
    // Class for the main form
    public partial class FrmMain : Form
    {
        private string Username;

        public FrmMain()
        {
            /*
            Constructor for FrmMain
            */

            InitializeComponent();
            this.Username = UsersHelper.LoggedInUsername;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            /*
            Method to load the main form
            */

            lblWelcomeMessage.Text = $"Welcome {this.Username}";
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            /*
            Method to log out the user on button click
            */

            UsersHelper.LoggedInUsername = "";

            FrmLogIn frmLogIn = new();
            frmLogIn.Show();
            this.Hide();
        }

        private void btnCreateBooking_Click(object sender, EventArgs e)
        {
            /*
            Method to open the create booking form on button click
            */

            FrmCreateBooking frmCreateBooking = new();
            frmCreateBooking.Show();
            this.Hide();
        }

        private void btnViewBookings_Click(object sender, EventArgs e)
        {
            /*
            Method to open the view bookings form on button click
            */

            FrmViewBookings frmViewBookings = new();
            frmViewBookings.ShowDialog();
        }

        private void btnViewCruises_Click(object sender, EventArgs e)
        {
            /*
            Method to open the view cruises form on button click
            */

            FrmViewCruises frmViewCruises = new();
            frmViewCruises.ShowDialog();
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            /*
            Method to open the create user form on button click
            */
            
            FrmCreateUser frmCreateUser = new();
            frmCreateUser.ShowDialog();
        }
    }
}
