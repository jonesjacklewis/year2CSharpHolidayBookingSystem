namespace OOPA2.Models
{
    // Class for TourModel
    public class TourModel
    {
        public DateTime TourStarDate { get; set; }

        public TourModel(DateTime tourStarDate)
        {
            /*
            Constructor for TourModel
            
            Parameters:
                tourStarDate (DateTime): The start date of the tour

            */
            TourStarDate = tourStarDate;
        }
    }
}
