namespace OOPA2.Models
{
    // Class for RoomTypeWithIdModel - inherits from RoomTypeModel
    public class RoomTypeWithIdModel : RoomTypeModel
    {
        public int Id { get; set; }
        
        public RoomTypeWithIdModel(int id, string roomName) : base(roomName)
        {
            /*
            Constructor for RoomTypeWithIdModel

            Parameters:
                id (int): The id of the room type
                roomName (string): The name of the room type
            */
            Id = id;
        }
    }
}
