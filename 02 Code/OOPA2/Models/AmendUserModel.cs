namespace OOPA2.Models
{
    // AmendUserModel class - Implements ICloneable
    public class AmendUserModel : ICloneable
    {
        public int UsersID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string? Telephone { get; set; }

        public AmendUserModel(int usersID, string email, string fullName, string? telephone)
        {
            /*
            Constructor for AmendUserModel class

            Parameters:
                usersID (int): The ID of the user
                email (string): The email of the user
                fullName (string): The full name of the user
                telephone (string): The telephone number of the user
            */
            UsersID = usersID;
            Email = email;
            FullName = fullName;
            Telephone = telephone;
        }

        public object Clone()
        {
            /*
            Clone method - Clones the AmendUserModel object
            */
            
            return this.MemberwiseClone();
        }
    }
}
