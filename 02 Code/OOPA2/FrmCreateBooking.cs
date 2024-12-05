using OOPA2.Database;
using OOPA2.Helpers;
using OOPA2.Models;

namespace OOPA2
{
    // Class for the Create Booking Form
    public partial class FrmCreateBooking : Form
    {

        private int price = 0;
        private int discountRate = 0;
        private readonly DatabaseController _dbController = new();

        public FrmCreateBooking()
        {
            /*
            Constructor for FrmCreateBooking
            */
            InitializeComponent();
            lblPriceValue.Text = $"£{price:F2}";
        }

        private void ReturnToMain()
        {
            /*
            Method to return to the main form
            */
            FrmMain frm = new();
            frm.Show();
            this.Hide();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            /*
            Method to return to the main form on button click
            */
            ReturnToMain();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            /*
            Method to book the room on button click
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

            string cardNumber = txtCardNumber.Text;
            cardNumber = cardNumber.Trim();
            cardNumber = cardNumber.Replace(" ", "");

            if (!cbBypassCardValidation.Checked)
            {
                ResponseModel isValidCard = UsersHelper.IsValidCreditCardNumber(cardNumber);

                if (!isValidCard.IsSuccess)
                {
                    MessageBox.Show($"Invalid Card - {isValidCard.Reason}");
                    return;
                }
            }

            object? roomObject = cmbRoom.SelectedItem;
            RoomTypeWithIdAndQuantityModel room;

            // https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching
            if (roomObject is RoomTypeWithIdAndQuantityModel model)
            {
                room = model;
            }
            else
            {
                MessageBox.Show("Something went wrong!");
                return;
            }

            this._dbController.AddUser(email, email + telephoneNumber, Enums.UserRolesEnum.Customer);
            int userId = this._dbController.GetUsersId(email);

            if (telephoneNumber.Length > 0)
            {
                this._dbController.AddUserContactDetails(userId, fullName, telephoneNumber);
            }
            else
            {
                this._dbController.AddUserContactDetails(userId, fullName);
            }

            this._dbController.ManageBooking(userId, room.Id, price);

            ReturnToMain();
        }

        private void FrmCreateBooking_Load(object sender, EventArgs e)
        {
            /*
            Method to load the form on load
            */
            List<TourWithIdModel> tours = _dbController.GetAllTourDatesWithIds();

            foreach (TourWithIdModel tour in tours)
            {
                cmbCruise.Items.Add(tour);
            }

            cbSingleOccupancy.Enabled = false;
        }

        private void cmbCruise_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            Method to handle the selected index change of the cmbCruise combobox
            */
            TourWithIdModel tour;

            if(cmbCruise.SelectedItem is TourWithIdModel model)
            {
                tour = model;
            }
            else
            {
                MessageBox.Show("Something went wrong");
                return;
            }

            List<RoomTypeWithIdAndQuantityModel> availableRooms = _dbController.GetAvailableRoomTypesByTour(tour.Id);

            cmbRoom.Items.Clear();

            foreach (RoomTypeWithIdAndQuantityModel room in availableRooms)
            {
                cmbRoom.Items.Add(room);
            }
        }

        private void cmbRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            Method to handle the selected index change of the cmbRoom combobox
            */
            RoomTypeWithIdAndQuantityModel room;

            if( cmbRoom.SelectedItem is RoomTypeWithIdAndQuantityModel model)
            {
                room = model;
            }
            else
            {
                MessageBox.Show("Something went wrong.");
                return;
            }
            
            this.price = _dbController.GetRoomDefaultPriceByRoomTypesToursId(room.Id);

            lblPriceValue.Text = string.Format("£{0:N2}", this.price / 100.0);
            cbSingleOccupancy.Enabled = true;
        }

        private void cbSingleOccupancy_CheckedChanged(object sender, EventArgs e)
        {
            /*
            Method to handle the checked change of the cbSingleOccupancy checkbox
            */
            RoomTypeWithIdAndQuantityModel room;

            if(cmbRoom.SelectedItem is RoomTypeWithIdAndQuantityModel model)
            {
                room = model;
            }
            else
            {
                MessageBox.Show("Something went wrong!");
                return;
            }

            if (cbSingleOccupancy.Checked)
            {
                this.discountRate = _dbController.GetRoomDiscountRateByRoomTypesToursId(room.Id);
                double discountAmount = this.price * (this.discountRate / 100.0);
                this.price -= (int)discountAmount;

            }
            else
            {
                this.price = _dbController.GetRoomDefaultPriceByRoomTypesToursId(room.Id);
            }

            lblPriceValue.Text = string.Format("£{0:N2}", this.price / 100.0);
        }

    }
}
