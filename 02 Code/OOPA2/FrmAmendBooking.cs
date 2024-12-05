using OOPA2.Database;
using OOPA2.Helpers;
using OOPA2.Models;

namespace OOPA2
{
    // Class for FrmAmendBooking - inherits from Form
    public partial class FrmAmendBooking : Form
    {
        private readonly DatabaseController _dbController = new();
        private AmendUserModel OriginalData;

        public FrmAmendBooking()
        {
            /*
            Constructor for FrmAmendBooking
            */
            InitializeComponent();

            if (UsersHelper.UserToAmendId == -1)
            {
                OriginalData = new(-1, String.Empty, String.Empty, null);
                MessageBox.Show("Something Went Wrong, Not UserId Supplied");
                this.Close();
                return;
            }

            OriginalData = _dbController.GetAmendUserDetailsByUserIs(UsersHelper.UserToAmendId);

            txtEmail.Text = OriginalData.Email;
            txtFullName.Text = OriginalData.FullName;

            if (OriginalData.Telephone is not null)
            {
                txtTelephone.Text = OriginalData.Telephone;
            }
        }

        private void btnAmend_Click(object sender, EventArgs e)
        {
            /*
            Method to amend the user details on button click
            */
            string fullName = txtFullName.Text;
            string telephoneNumber = txtTelephone.Text.Trim();

            if (telephoneNumber.Length > 0)
            {
                ResponseModel isValidTelephoneNumber = UsersHelper.IsValidTelephone(telephoneNumber);

                if (!isValidTelephoneNumber.IsSuccess)
                {
                    MessageBox.Show($"Invalid Telephone Number - {isValidTelephoneNumber.Reason}");
                    return;
                }
            }

            string email = txtEmail.Text.Trim();

            ResponseModel isValidEmail = UsersHelper.IsValidEmail(email);

            if (!isValidEmail.IsSuccess)
            {
                MessageBox.Show($"Invalid Email Address - {isValidEmail.Reason}");
                return;
            }

            AmendUserModel newData = (AmendUserModel)OriginalData.Clone();

            newData.Email = email;
            newData.FullName = fullName;

            if (telephoneNumber.Length > 0)
            {
                newData.Telephone = telephoneNumber;
                _dbController.UpdateBookingInfo(newData);
            }
            else
            {
                newData.Telephone = null;
                _dbController.UpdateBookingInfoNullTelephone(newData);
            }

            this.Close();

        }
    }
}
