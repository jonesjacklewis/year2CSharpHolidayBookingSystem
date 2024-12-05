namespace OOPA2.Models
{
    // Class for TourWithIdModel - inherits from TourModel
    public class TourWithIdModel: TourModel
    {
        public int Id { get; set; }

        public TourWithIdModel(int id, DateTime tourStarDate) : base(tourStarDate)
        {
            /*
            Constructor for TourWithIdModel

            Parameters:
                id (int): The id of the tour
                tourStarDate (DateTime): The start date of the tour
            
            */
            this.Id = id;
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
