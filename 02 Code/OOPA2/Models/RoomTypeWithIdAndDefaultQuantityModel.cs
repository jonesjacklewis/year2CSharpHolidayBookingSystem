namespace OOPA2.Models
{
    // Class for RoomTypeWithIdAndDefaultQuantityModel - inherits from RoomTypeWithIdModel
    public class RoomTypeWithIdAndDefaultQuantityModel : RoomTypeWithIdModel
    {
        public int DefaultQuantity { get; set; }
        public RoomTypeWithIdAndDefaultQuantityModel(int defaultQuantity, int id, string roomName) : base(id, roomName)
        {
            /*
            Constructor for RoomTypeWithIdAndDefaultQuantityModel
            
            Parameters:
                defaultQuantity (int): The default quantity of the room type
                id (int): The id of the room type
                roomName (string): The name of the room type
            
            */
            this.DefaultQuantity = defaultQuantity;
        }
    }
}
