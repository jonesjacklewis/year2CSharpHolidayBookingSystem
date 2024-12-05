namespace OOPA2.Models
{
    // RoomTypeModel class - Represents a room type
    public class RoomTypeModel
    {
        public string RoomName { get; set; }

        public RoomTypeModel(string roomName)
        {
            /*
            Constructor for RoomTypeModel class

            Parameters:
                roomName (string): The name of the room type
            */

            RoomName = roomName;
        }
    }
}
