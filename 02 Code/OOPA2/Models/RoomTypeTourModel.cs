namespace OOPA2.Models
{
    // Class for RoomTypeTourModel
    public class RoomTypeTourModel
    {
        public int ToursId { get; set; }
        public int RoomTypesId { get; set; }

        public RoomTypeTourModel(int toursId, int roomTypesId)
        {
            /*
            Constructor for RoomTypeTourModel

            Parameters:
                toursId (int): The id of the tour
                roomTypesId (int): The id of the room type
            */
            ToursId = toursId;
            RoomTypesId = roomTypesId;
        }
    }
}
