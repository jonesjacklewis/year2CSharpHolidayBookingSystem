using OOPA2.Database;
using OOPA2.Helpers;
using OOPA2.Models;

namespace OOPA2
{
    // Class for FrmViewCruises - inherits from Form
    public partial class FrmViewCruises : Form
    {
        private readonly DatabaseController _dbController = new();

        public FrmViewCruises()
        {
            /*
            Constructor for FrmViewCruises
            */

            InitializeComponent();
        }

        private void SetUpDataGrid()
        {
            /*
            Method to set up the data grid view
            */

            List<ViewTourModel> tours = _dbController.GetViewTourModels();

            // Make uneditable
            dgvCruises.ReadOnly = true;

            // Add Columns
            dgvCruises.Columns.Add("dateColumn", "Date");
            dgvCruises.Columns.Add("incomeColumn", "Income (£)");
            dgvCruises.Columns.Add("roomsRemainingColumn", "Rooms Remaining");

            // Button Column
            DataGridViewButtonColumn btnViewBookings = new()
            {
                Name = "viewBookings",
                Text = "View Bookings",
                HeaderText = "View Bookings",
                UseColumnTextForButtonValue = true
            };
            dgvCruises.Columns.Add(btnViewBookings);


            // Add a dummy row for temporary purposes

            foreach (ViewTourModel tour in tours)
            {
                int rowIndex = dgvCruises.Rows.Add();

                dgvCruises.Rows[rowIndex].Cells["dateColumn"].Value = tour;
                dgvCruises.Rows[rowIndex].Cells["incomeColumn"].Value = string.Format("{0:N2}", tour.TotalIncomePence / 100.0);
                dgvCruises.Rows[rowIndex].Cells["roomsRemainingColumn"].Value = tour.RoomsRemaining;


                dgvCruises.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                dgvCruises.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                dgvCruises.Rows[rowIndex].Cells["viewBookings"].Tag = tour.Id;
                dgvCruises.Refresh();
            }
        }

        private void FrmViewCruises_Load(object sender, EventArgs e)
        {
            /*
            Method to load the view cruises form
            */

            SetUpDataGrid();
        }

        private void dgvCruises_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            Method to handle the cell content click event
            */

            // Check if the click is on the button column
            if (e.ColumnIndex == dgvCruises.Columns["viewBookings"].Index && e.RowIndex >= 0)
            {
                // Get tour id from the tag
                int tourId = (int)dgvCruises.Rows[e.RowIndex].Cells["viewBookings"].Tag;

                UsersHelper.SelectedTourId = tourId;

                FrmViewBookings frmViewBookings = new();
                this.Hide();
                frmViewBookings.ShowDialog();
            }
        }
    }
}
