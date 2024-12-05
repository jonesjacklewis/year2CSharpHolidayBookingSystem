using OOPA2.Database;
using OOPA2.Helpers;
using OOPA2.Models;

namespace OOPA2
{
    // Class for FrmViewBookings - inherits from Form
    public partial class FrmViewBookings : Form
    {
        private readonly DatabaseController _dbController = new();

        public FrmViewBookings()
        {
            /*
            Constructor for FrmViewBookings
            */

            InitializeComponent();
            btnClearSearch.Visible = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            /*
            Method to handle the text changed event for the search text box
            */

            string strippedSearchText = txtSearch.Text.Trim().ToLower();

            btnClearSearch.Visible = strippedSearchText.Length > 0;
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            /*
            Method to handle the clear search button click event
            */

            btnClearSearch.Visible = false;
            txtSearch.Text = "";

            foreach (DataGridViewRow row in dgvBookings.Rows)
            {
                row.Visible = true;
            }
        }

        private void SetUpDataGrid()
        {
            /*
            Method to set up the data grid view
            */

            List<ViewBookingModel> bookings;

            if (UsersHelper.SelectedTourId != -1)
            {
                bookings = _dbController.GetAllBookingsByTourId(UsersHelper.SelectedTourId);
            }
            else
            {
                bookings = _dbController.GetAllBookings();
            }

            // Make uneditable
            dgvBookings.ReadOnly = true;

            // Adding Columns
            dgvBookings.Columns.Add("emailColumn", "Email");
            dgvBookings.Columns.Add("fullNameColumn", "Full Name");
            dgvBookings.Columns.Add("telephoneColumn", "Telephone");
            dgvBookings.Columns.Add("bookingPriceColumn", "Booking Price (£)");
            dgvBookings.Columns.Add("tourStateDateColumn", "Tour Start Date");
            dgvBookings.Columns.Add("roomNameColumn", "Room Name");

            // Button Columns
            DataGridViewButtonColumn btnCancelColumn = new()
            {
                Name = "cancelColumn",
                Text = "Cancel",
                HeaderText = "Cancel",
                UseColumnTextForButtonValue = true
            };
            dgvBookings.Columns.Add(btnCancelColumn);

            DataGridViewButtonColumn btnAmendColumn = new()
            {
                Name = "amendColumn",
                Text = "Amend",
                HeaderText = "Amend",
                UseColumnTextForButtonValue = true
            };
            dgvBookings.Columns.Add(btnAmendColumn);

            if (bookings.Count == 0)
            {
                dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                dgvBookings.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

                dgvBookings.Refresh();
            }

            foreach (ViewBookingModel booking in bookings)
            {
                int rowIndex = dgvBookings.Rows.Add();

                dgvBookings.Rows[rowIndex].Cells["emailColumn"].Value = booking.Email;
                dgvBookings.Rows[rowIndex].Cells["fullNameColumn"].Value = booking.FullName;
                dgvBookings.Rows[rowIndex].Cells["telephoneColumn"].Value = booking.Telephone;
                dgvBookings.Rows[rowIndex].Cells["bookingPriceColumn"].Value = string.Format("{0:N2}", booking.BookingPrice / 100.0);
                dgvBookings.Rows[rowIndex].Cells["tourStateDateColumn"].Value = booking;
                dgvBookings.Rows[rowIndex].Cells["roomNameColumn"].Value = booking.RoomName;
                dgvBookings.Rows[rowIndex].Cells["cancelColumn"].Tag = booking.BookingsId;
                dgvBookings.Rows[rowIndex].Cells["amendColumn"].Tag = booking.BookingsId;


                dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                dgvBookings.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

                dgvBookings.Refresh();
            }
        }

        private void FrmViewBookings_Load(object sender, EventArgs e)
        {
            /*
            Method to load the view bookings form
            */

            btnResetTourFilter.Visible = UsersHelper.SelectedTourId != -1;
            SetUpDataGrid();
        }

        private void HandleCancellation(int bookingId)
        {
            /*
            Method to handle the cancellation of a booking
            */

            DateTime tourDateForBooking = _dbController.GetDateOfTourByBookingId(bookingId);

            // Get today's date
            DateTime today = DateTime.Now.Date;

            // Calculate the date 10 days from now
            DateTime dateTenDaysFromNow = today.AddDays(10);

            // Check if the tour date is greater than or equal to 10 days from now
            bool canRefund = tourDateForBooking >= dateTenDaysFromNow;

            if (canRefund)
            {
                DialogResult response = MessageBox.Show("Are you sure you want to cancel this booking? A 50% refund will be applied", "Booking Cancellation Confirmation (Refund)", MessageBoxButtons.YesNo);

                if (response == DialogResult.Yes)
                {
                    _dbController.UpdateBooking(bookingId, Enums.BookingStatesEnum.Refunded);
                    this.Close();
                }
            }
            else
            {
                DialogResult response = MessageBox.Show("Are you sure you want to cancel this booking? It is not possible to refund.", "Booking Cancellation Confirmation (No Refund)", MessageBoxButtons.YesNo);

                if (response == DialogResult.Yes)
                {
                    _dbController.UpdateBooking(bookingId, Enums.BookingStatesEnum.Cancelled);
                    this.Close();
                }
            }
        }

        private void dgvBookings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            Method to handle the cell content click event for the data grid view
            */
            
            // Retrieve the total income from the Tag property of the button cell
            int bookingId = (int)dgvBookings.Rows[e.RowIndex].Cells["cancelColumn"].Tag;

            // Cancellation / Refund Handling
            if (e.ColumnIndex == dgvBookings.Columns["cancelColumn"].Index && e.RowIndex >= 0)
            {
                HandleCancellation(bookingId);
                return;
            }

            // Handle Amend
            if (e.ColumnIndex == dgvBookings.Columns["amendColumn"].Index && e.RowIndex >= 0)
            {
                UsersHelper.UserToAmendId = _dbController.GetUserIdByBookingId(bookingId);

                FrmAmendBooking frmAmendBooking = new();
                this.Hide();
                frmAmendBooking.ShowDialog();
            }

        }

        private void btnResetTourFilter_Click(object sender, EventArgs e)
        {
            /*
            Method to reset the tour filter on button click
            */

            UsersHelper.SelectedTourId = -1;
            FrmViewBookings frmViewBookings = new();
            this.Hide();
            frmViewBookings.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            /*
            Method to handle the search button click event
            */
            
            string searchTerm = txtSearch.Text.ToLower().Trim();
            if (searchTerm == String.Empty)
            {
                txtSearch.Text = "";
                return;
            }
            foreach (DataGridViewRow row in dgvBookings.Rows)
            {

                string emailValue = row.Cells["emailColumn"].Value?.ToString()?.ToLower() ?? string.Empty;
                string fullNameValue = row.Cells["fullNameColumn"].Value?.ToString()?.ToLower() ?? string.Empty;
                string telephone = row.Cells["telephoneColumn"].Value?.ToString()?.ToLower() ?? string.Empty;
                string date = row.Cells["tourStateDateColumn"].Value?.ToString()?.ToLower() ?? string.Empty;
                string roomName = row.Cells["roomNameColumn"].Value?.ToString()?.ToLower() ?? string.Empty;


                if (!(
                    emailValue.Contains(searchTerm) ||
                    fullNameValue.Contains(searchTerm) ||
                    telephone.Contains(searchTerm) ||
                    date.Contains(searchTerm) ||
                    roomName.Contains(searchTerm)
                    )
                )
                {
                    row.Visible = false;
                }
            }
        }
    }
}
