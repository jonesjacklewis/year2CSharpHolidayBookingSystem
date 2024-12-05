namespace OOPA2.Models
{
    // Class for RoomTypeWithIdAndQuantityModel - inherits from RoomTypeModel
    public class RoomTypeWithIdAndQuantityModel : RoomTypeModel
    {

        public int Id { get; set; }
        public int Quantity { get; set; }

        public RoomTypeWithIdAndQuantityModel(int id, int quantity, string roomName) : base(roomName)
        {
            /*
            Constructor for RoomTypeWithIdAndQuantityModel

            Parameters:
                id (int): The id of the room type
                quantity (int): The quantity of the room type
                roomName (string): The name of the room type
            */

            this.Id = id;
            this.Quantity = quantity;
        }

        public override string ToString()
        {
            /*
            Method to return the name of the room type

            Returns:
                string: The name of the room type
            */
            return this.RoomName;
        }
    }
}
