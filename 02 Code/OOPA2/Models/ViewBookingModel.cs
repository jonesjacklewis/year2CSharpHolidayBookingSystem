namespace OOPA2.Models
{
    // Class for ViewBookingModel
    public class ViewBookingModel
    {
        public int BookingsId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string? Telephone { get; set; }
        public int BookingPrice { get; set; }
        public DateTime TourStartDate { get; set; }
        public string RoomName { get; set; }

        public ViewBookingModel(int bookingsId, string email, string fullName, string? telephone, int bookingPrice, DateTime tourStartDate, string roomName)
        {
            /*
            Constructor for ViewBookingModel

            Parameters:
                bookingsId (int): The id of the booking
                email (string): The email of the customer
                fullName (string): The full name of the customer
                telephone (string): The telephone number of the customer
                bookingPrice (int): The price of the booking
                tourStartDate (DateTime): The start date of the tour
                roomName (string): The name of the room
            */

            BookingsId = bookingsId;
            Email = email;
            FullName = fullName;
            Telephone = telephone;
            BookingPrice = bookingPrice;
            TourStartDate = tourStartDate;
            RoomName = roomName;
        }

        public override string ToString()
        {
            /*
            Method to return the start date of the tour

            Returns:
                string: The start date of the tour
            */
            
            return this.TourStartDate.ToString("yyyy-MM-dd");
        }

    }
}
