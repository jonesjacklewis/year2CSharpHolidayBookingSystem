namespace OOPA2.Models
{
    // Class for ViewTourModel - inherits from TourWithIdModel
    public class ViewTourModel : TourWithIdModel
    {
        public int TotalIncomePence { get; set; }
        public int RoomsRemaining { get; set; }

        public ViewTourModel(int totalIncomePence, int roomsRemaining, int id, DateTime tourStarDate) : base(id, tourStarDate)
        {
            /*
            Constructor for ViewTourModel

            Parameters:
                totalIncomePence (int): The total income in pence
                roomsRemaining (int): The number of rooms remaining
                id (int): The id of the tour
                tourStarDate (DateTime): The start date of the tour
            
            */

            this.TotalIncomePence = totalIncomePence;
            this.RoomsRemaining = roomsRemaining;
        }

        public override string ToString()
        {
            /*
            Method to return the start date of the tour

            Returns:
                string: The start date of the tour
            */
            
            return this.TourStarDate.ToString("yyyy-MM-dd");
        }

    }
}
