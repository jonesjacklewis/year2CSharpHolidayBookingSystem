using Microsoft.Data.Sqlite;
using OOPA2.Enums;
using OOPA2.Helpers;
using OOPA2.Models;
using System.Text;

namespace OOPA2.Database
{
    // DatabaseController Class - Implements IDisposable
    public class DatabaseController : IDisposable
    {
        private SqliteConnection _connection;

        public DatabaseController(string filename = "database.db")
        {
            /*
            Constructor for the DatabaseController class.

            Parameters:
                filename (string): The name of the file to use as the database. Default is "database.db".
            */
            _connection = new SqliteConnection($"Data Source={filename}");

            CreateTablesIfNotExists();
            AddDefaultData();
        }
      

        private void CreateTablesIfNotExists()
        {
            /*
            Creates the tables if they do not already exist.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            string createTableSqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "createTables.sql");

            if (!File.Exists(createTableSqlFile))
            {
                _connection.Close();
                throw new FileNotFoundException("The createTables.sql file is not available");
            }

            string content = File.ReadAllText(createTableSqlFile);


            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = content;

            command.ExecuteNonQuery();

            _connection.Close();
        }

        private void AddUserRoles()
        {
            /*
            Adds the default user roles to the database. The default user roles are "Admin" and "Customer".
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            string[] userRoles =
            {
                "Admin",
                "Customer"
            };

            foreach (string userRole in userRoles)
            {
                SqliteCommand command = _connection.CreateCommand();

                // Command Parameterisation
                command.CommandText = "INSERT OR IGNORE INTO UserRoles (RoleName) VALUES (@roleName);";

                command.Parameters.AddWithValue("@roleName", userRole);

                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public int GetUserRoleId(UserRolesEnum role)
        {
            /*
            Gets the UserRolesId for a given UserRolesEnum.

            Parameters:
                role (UserRolesEnum): The UserRolesEnum to get the UserRolesId for.
            Returns:
                int: The UserRolesId for the given UserRolesEnum.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT UserRolesId FROM UserRoles WHERE RoleName = @roleName LIMIT 1";

            command.Parameters.AddWithValue("@roleName", role.ToString());

            object? objId = command.ExecuteScalar();

            _connection.Close();

            if (objId is null)
            {
                return -1;
            }

            int id = Int32.Parse(objId.ToString() ?? "-1");

            return id;
        }

        private void InsertIntoUsers(string username, UserRolesEnum roleType)
        {
            /*
            Inserts a new user into the Users table.

            Parameters:
                username (string): The username of the user to insert.
                roleType (UserRolesEnum): The role of the user to insert.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int roleId = GetUserRoleId(roleType);

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }


            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT OR IGNORE INTO Users (UsernameOrEmail, UserRolesId) VALUES (@username, @roleType);";

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@roleType", roleId);

            command.ExecuteNonQuery();

            _connection.Close();
        }

        public AmendUserModel GetAmendUserDetailsByUserIs(int userId)
        {
            /*
            Gets the amend user details for a given userId.

            Parameters:
                userId (int): The userId to get the amend user details for.
            Returns:
                AmendUserModel: The amend user details for the given userId.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT U.UsernameOrEmail, UCD.FullName, UCD.Telephone FROM Users U JOIN UserContactDetails UCD ON U.UsersId = UCD.UsersId WHERE U.UsersId = @userId";

            command.Parameters.AddWithValue("@userId", userId);

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                string email = reader.GetString(0);
                string fullName = reader.GetString(1);

                string? telephone = null;

                if (!reader.IsDBNull(2))
                {
                    telephone = reader.GetString(2);
                }

                AmendUserModel model = new(userId, email, fullName, telephone);

                _connection.Close();
                return model;
            }
            _connection.Close();
            throw new InvalidOperationException("Something went wrong getting the amend user details");

        }

        public int GetUsersId(string username)
        {
            /*
            Gets the UsersId for a given username.

            Parameters:
                username (string): The username to get the UsersId for.
            Returns:
                int: The UsersId for the given username.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT UsersId FROM Users WHERE UsernameOrEmail = @username LIMIT 1";

            command.Parameters.AddWithValue("@username", username);

            Object? objId = command.ExecuteScalar();

            _connection.Close();

            if (objId is null)
            {
                return -1;
            }

            int id = Int32.Parse(objId.ToString() ?? "");

            return id;
        }

        private int GetUserCredentialsId(int usersId)
        {
            /*
            Gets the UserCredentialsId for a given UsersId.

            Parameters:
                usersId (int): The UsersId to get the UserCredentialsId for.
            Returns:
                int: The UserCredentialsId for the given UsersId.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT UserCredentialsId FROM UserCredentials WHERE UsersId = @usersId LIMIT 1";

            command.Parameters.AddWithValue("@usersId", usersId);

            Object? objId = command.ExecuteScalar();

            _connection.Close();

            if (objId is null)
            {
                return -1;
            }

            int id = Int32.Parse(objId.ToString() ?? "");

            return id;
        }

        private void DecrementCurrentRoomQuntity(int roomTypeTourId, int amount = 1)
        {
            /*
            Decrements the CurrentQuantity for a given RoomTypesToursId by a given amount.

            Parameters:
                roomTypeTourId (int): The RoomTypesToursId to decrement the CurrentQuantity for.
                amount (int): The amount to decrement the CurrentQuantity by. Default is 1.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE RoomTypesTours SET CurrentQuantity = CurrentQuantity - @amount WHERE RoomTypesToursId = @roomTypeTourId";

            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@roomTypeTourId", roomTypeTourId);

            command.ExecuteNonQuery();
            _connection.Close();

        }

        private RoomTypeTourModel GetRoomTypeIdAndTour(int roomTypeTourId)
        {
            /*
            Gets a RoomTypeTourModel for a given RoomTypesToursId.

            Parameters:
                roomTypeTourId (int): The RoomTypesToursId to get the RoomTypeTourModel for.
            Returns:
                RoomTypeTourModel: The RoomTypeTourModel for the given RoomTypesToursId.
            */
            
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT ToursId, RoomTypesId FROM RoomTypesTours WHERE RoomTypesToursId = @roomTypeTourId LIMIT 1";

            command.Parameters.AddWithValue("@roomTypeTourId", roomTypeTourId);

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int toursId = reader.GetInt32(0);
                int roomTypesId = reader.GetInt32(1);

                RoomTypeTourModel model = new(toursId, roomTypesId);

                return model;
            }
            {
                throw new KeyNotFoundException("Specified id does not have a matching record.");
            }

        }

        public void ManageBooking(int userId, int roomTypeTourId, int bookingPrice)
        {
            /*
            Manages a booking for a given userId, roomTypeTourId and bookingPrice.

            Parameters:
                userId (int): The userId to manage the booking for.
                roomTypeTourId (int): The RoomTypesToursId to manage the booking for.
                bookingPrice (int): The price of the booking.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int bookingStateId = GetBookingStateId(BookingStatesEnum.Booked);
            RoomTypeTourModel roomTypeTour = GetRoomTypeIdAndTour(roomTypeTourId);


            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();

            StringBuilder sb = new();

            sb.AppendLine("INSERT INTO Bookings");
            sb.AppendLine("(");
            sb.AppendLine("UsersId,");
            sb.AppendLine("ToursId,");
            sb.AppendLine("RoomTypesId,");
            sb.AppendLine("BookingStatesId,");
            sb.AppendLine("BookingPrice");
            sb.AppendLine(")");
            sb.AppendLine("VALUES");
            sb.AppendLine("(@userId, @toursId, @roomTypesId, @bookingStatesId, @bookingPrice);");

            command.CommandText = sb.ToString();

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@toursId", roomTypeTour.ToursId);
            command.Parameters.AddWithValue("@roomTypesId", roomTypeTour.RoomTypesId);
            command.Parameters.AddWithValue("@bookingStatesId", bookingStateId);
            command.Parameters.AddWithValue("@bookingPrice", bookingPrice);

            command.ExecuteNonQuery();

            DecrementCurrentRoomQuntity(roomTypeTourId);

            _connection.Close();
        }

        public void AddUserContactDetails(int userId, string fullName, string? telephone = null)
        {
            /*
            Adds user contact details for a given userId, fullName and telephone.

            Parameters:
                userId (int): The userId to add the user contact details for.
                fullName (string): The full name to add to the user contact details.
                telephone (string): The telephone to add to the user contact details. Default is null.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();


            if (telephone is null)
            {
                command.CommandText = "INSERT OR IGNORE INTO UserContactDetails (Fullname, UsersId) VALUES (@fullName, @usersId);";

                command.Parameters.AddWithValue("@fullName", fullName);
                command.Parameters.AddWithValue("@usersId", userId);
            }
            else
            {
                command.CommandText = "INSERT OR IGNORE INTO UserContactDetails (Fullname, UsersId, Telephone) VALUES (@fullName, @usersId, @telephone);";

                command.Parameters.AddWithValue("@fullName", fullName);
                command.Parameters.AddWithValue("@usersId", userId);
                command.Parameters.AddWithValue("@telephone", telephone);
            }

            command.ExecuteNonQuery();
            _connection.Close();
        }

        private void InsertIntoUserCredentials(string username, string hashedPassword)
        {
            /*
            Inserts a new user credentials into the UserCredentials table.

            Parameters:
                username (string): The username of the user to insert.
                hashedPassword (string): The hashed password of the user to insert. Uses SHA256.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int userId = GetUsersId(username);

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT OR IGNORE INTO UserCredentials (UsersId, HashedPassword) VALUES (@userId, @hashedPassword);";

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@hashedPassword", hashedPassword);

            command.ExecuteNonQuery();

            _connection.Close();
        }

        public void AddUser(string username, string rawPassword, UserRolesEnum roleType)
        {
            /*
            Adds a new user to the database.

            Parameters:
                username (string): The username of the user to add.
                rawPassword (string): The raw password of the user to add.
                roleType (UserRolesEnum): The role of the user to add.
            */

            string hashedPassword = UsersHelper.HashPassword(rawPassword);

            InsertIntoUsers(username, roleType);
            InsertIntoUserCredentials(username, hashedPassword);
        }

        private void AddDefaultAdminUser()
        {
            /*
            Adds the default admin user to the database.
            */
            string username = "DefaultAdmin";
            string rawPassword = "DefaultAdminPass123!";

            AddUser(username, rawPassword, UserRolesEnum.Admin);

        }

        private void AddDefaultCustomerUser()
        {
            /*
            Adds the default customer user to the database.
            */
            string username = "DefaultCustomer";
            string rawPassword = "DefaultCustomerPass123!";

            AddUser(username, rawPassword, UserRolesEnum.Customer);

        }

        private void AddDefaultTourDates()
        {
            /*
            Adds the default tour dates to the database.
            */
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            string[] datesAsStrings = {
                "2025-06-06",
                "2025-06-13",
                "2025-06-20",
                "2025-06-27"
            };

            DateTime[] dates = datesAsStrings.Select(dateString => DateTime.Parse(dateString)).ToArray();

            foreach (DateTime date in dates)
            {
                SqliteCommand command = _connection.CreateCommand();

                command.CommandText = "INSERT OR IGNORE INTO Tours (TourStartDate) VALUES (@date)";

                command.Parameters.AddWithValue("@date", date);
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public int GetTourId(DateTime date)
        {
            /*
            Gets the ToursId for a given date.

            Parameters:
                date (DateTime): The date to get the ToursId for.
            Returns:
                int: The ToursId for the given date.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT ToursId FROM Tours WHERE TourStartDate = @date";

            command.Parameters.AddWithValue("@date", date);

            object? idObject = command.ExecuteScalar();

            _connection.Close();

            if (idObject is null)
            {
                return -1;
            }

            int id = Int32.Parse(idObject.ToString() ?? "");

            return id;

        }

        private void AddDefaultRoomTypes()
        {
            /*
            Adds the default room types to the database.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            string[] roomTypes =
            {
                "Penthouse",
                "Luxury",
                "Standard",
                "Budget"
            };

            foreach (string roomType in roomTypes)
            {
                SqliteCommand command = _connection.CreateCommand();

                command.CommandText = "INSERT OR IGNORE INTO RoomTypes (RoomName) VALUES (@RoomName)";
                command.Parameters.AddWithValue("@RoomName", roomType);

                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        private List<RoomTypeWithIdModel> GetAllRoomTypes()
        {
            /*
            Gets all room types from the database.

            Returns:
                List<RoomTypeWithIdModel>: A list of RoomTypeWithIdModel objects.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
            List<RoomTypeWithIdModel> roomTypes = new();

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT RoomTypesId, RoomName FROM RoomTypes";

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string roomName = reader.GetString(1);

                RoomTypeWithIdModel roomType = new RoomTypeWithIdModel(id, roomName);

                roomTypes.Add(roomType);
            }

            _connection.Close();

            return roomTypes;
        }

        private List<RoomTypeWithIdAndDefaultQuantityModel> GetAllRoomTypesWithDefaultQuantities()
        {
            /*
            Gets all room types with default quantities from the database.

            Returns:
                List<RoomTypeWithIdAndDefaultQuantityModel>: A list of RoomTypeWithIdAndDefaultQuantityModel objects.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
            List<RoomTypeWithIdAndDefaultQuantityModel> roomTypes = new();

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT rt.RoomTypesId, rt.RoomName, rci.DefaultQuantity FROM RoomTypes rt JOIN RoomCostInfo rci ON rt.RoomTypesId = rci.RoomTypesId";

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string roomName = reader.GetString(1);
                int defaultQuantity = reader.GetInt32(2);

                RoomTypeWithIdAndDefaultQuantityModel roomType = new(defaultQuantity, id, roomName);

                roomTypes.Add(roomType);
            }

            _connection.Close();

            return roomTypes;
        }

        private void SetUpRoomCostInfo()
        {
            /*
            Sets up the room cost info in the database.
            */
            
            List<RoomTypeWithIdModel> roomsTypes = GetAllRoomTypes();

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            foreach (RoomTypeWithIdModel roomType in roomsTypes)
            {
                int defaultQuantity = -1;
                int currentPricePence = -1;
                int discountRate = -1;
                switch (roomType.RoomName)
                {
                    case "Penthouse":
                        // Code for Penthouse
                        defaultQuantity = 1;
                        currentPricePence = 785 * 100;
                        discountRate = 20;
                        break;
                    case "Luxury":
                        // Code for Luxury
                        defaultQuantity = 2;
                        currentPricePence = 565 * 100;
                        discountRate = 18;
                        break;
                    case "Standard":
                        // Code for Standard
                        defaultQuantity = 5;
                        currentPricePence = 450 * 100;
                        discountRate = 15;
                        break;
                    case "Budget":
                        // Code for Budget
                        defaultQuantity = 8;
                        currentPricePence = 350 * 100;
                        discountRate = 10;
                        break;
                    default:
                        // Code for unrecognized room type
                        break;
                }

                SqliteCommand command = _connection.CreateCommand();
                command.CommandText = "INSERT OR IGNORE INTO RoomCostInfo (RoomTypesId, DefaultQuantity, CurrentPricePence, DiscountRate) VALUES (@id, @defaultQuantity, @currentPricePence, @discountRate);";

                command.Parameters.AddWithValue("@id", roomType.Id);
                command.Parameters.AddWithValue("@defaultQuantity", defaultQuantity);
                command.Parameters.AddWithValue("@currentPricePence", currentPricePence);
                command.Parameters.AddWithValue("@discountRate", discountRate);

                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public int GetRoomId(RoomTypesEnum roomType)
        {
            /*
            Gets the RoomTypesId for a given RoomTypesEnum.

            Parameters:
                roomType (RoomTypesEnum): The RoomTypesEnum to get the RoomTypesId for.
            Returns:
                int: The RoomTypesId for the given RoomTypesEnum.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT RoomTypesId FROM RoomTypes WHERE RoomName = @roomName LIMIT 1;";

            command.Parameters.AddWithValue("@roomName", roomType.ToString());

            object? roomIdObj = command.ExecuteScalar();

            _connection.Close();

            if (roomIdObj is null)
            {
                return -1;
            }

            int roomId = Int32.Parse(roomIdObj.ToString() ?? "");

            return roomId;
        }

        public int GetCostOfRoomInPence(RoomTypesEnum roomType)
        {
            /*
            Gets the cost of a room in pence for a given RoomTypesEnum.

            Parameters:
                roomType (RoomTypesEnum): The RoomTypesEnum to get the cost of the room in pence for.
            Returns:
                int: The cost of the room in pence for the given RoomTypesEnum.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int roomId = GetRoomId(roomType);

            if (roomId == -1)
            {
                return -1;
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT CurrentPricePence FROM RoomCostInfo WHERE RoomTypesId = @id;";

            command.Parameters.AddWithValue("@id", roomId);

            object? pricePenceObj = command.ExecuteScalar();

            _connection.Close();

            if (pricePenceObj is null)
            {
                return -1;
            }

            int pricePence = Int32.Parse(pricePenceObj.ToString() ?? "");

            return pricePence;
        }

        private List<int> GetAllTourIds()
        {
            /*
            Gets all tour ids from the database.

            Returns:
                List<int>: A list of tour ids.
            */
            
            List<int> tourIds = new();

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT ToursId FROM Tours";

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                tourIds.Add(id);
            }

            _connection.Close();

            return tourIds;
        }

        public List<TourWithIdModel> GetAllTourDatesWithIds()
        {
            /*
            Gets all tour dates with ids from the database.

            Returns:
                List<TourWithIdModel>: A list of TourWithIdModel objects.
            */

            List<TourWithIdModel> tours = new();

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT ToursId, TourStartDate FROM Tours";

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                DateTime tourStartDate = reader.GetDateTime(1);

                TourWithIdModel tourWithIdModel = new(id, tourStartDate);

                tours.Add(tourWithIdModel);
            }

            _connection.Close();

            return tours;
        }

        public int GetRoomDefaultPriceByRoomTypesToursId(int rttId)
        {
            /*
            Gets the default price of a room by RoomTypesToursId.

            Parameters:
                rttId (int): The RoomTypesToursId to get the default price for.
            Returns:
                int: The default price of the room.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            string query = "SELECT rci.CurrentPricePence FROM RoomCostInfo rci JOIN RoomTypesTours rtt ON rci.RoomTypesId = rtt.RoomTypesId WHERE rtt.RoomTypesToursId = @rttId";

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@rttId", rttId);

            object? defaultPriceObj = command.ExecuteScalar();

            _connection.Close();

            if (defaultPriceObj is null)
            {
                return -1;
            }

            int defaultPrice = Int32.Parse(defaultPriceObj.ToString() ?? "");

            return defaultPrice;
        }

        public int GetRoomDiscountRateByRoomTypesToursId(int rttId)
        {
            /*
            Gets the discount rate of a room by RoomTypesToursId.

            Parameters:
                rttId (int): The RoomTypesToursId to get the discount rate for.
            Returns:
                int: The discount rate of the room.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            string query = "SELECT rci.DiscountRate FROM RoomCostInfo rci JOIN RoomTypesTours rtt ON rci.RoomTypesId = rtt.RoomTypesId WHERE rtt.RoomTypesToursId = @rttId";

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@rttId", rttId);

            object? discountRateObj = command.ExecuteScalar();

            _connection.Close();

            if (discountRateObj is null)
            {
                return -1;
            }

            int defaultPrice = Int32.Parse(discountRateObj.ToString() ?? "");

            return defaultPrice;
        }

        public List<RoomTypeWithIdAndQuantityModel> GetAvailableRoomTypesByTour(int tourId)
        {
            /*
            Gets the available room types by tour.

            Parameters:
                tourId (int): The tour id to get the available room types for.
            Returns:
                List<RoomTypeWithIdAndQuantityModel>: A list of RoomTypeWithIdAndQuantityModel objects.
            */
            
            List<RoomTypeWithIdAndQuantityModel> tours = new();

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            string query = "SELECT rtt.RoomTypesToursId, rt.RoomName, rtt.CurrentQuantity FROM RoomTypesTours rtt JOIN RoomTypes rt ON rtt.RoomTypesId = rt.RoomTypesId WHERE rtt.ToursId = @tourId AND rtt.CurrentQuantity > 0;";

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@tourId", tourId);

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string roomName = reader.GetString(1);
                int currentQuantity = reader.GetInt32(2);

                RoomTypeWithIdAndQuantityModel model = new(id, currentQuantity, roomName);

                tours.Add(model);
            }


            _connection.Close();

            return tours;
        }

        private void SetUpRoomTypesToursInfo()
        {
            /*
            Sets up the room types tours info in the database.
            */
            
            List<int> tourIds = GetAllTourIds();
            List<RoomTypeWithIdAndDefaultQuantityModel> roomsTypes = GetAllRoomTypesWithDefaultQuantities();

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            foreach (int tourId in tourIds)
            {
                foreach (RoomTypeWithIdAndDefaultQuantityModel roomType in roomsTypes)
                {
                    SqliteCommand command = _connection.CreateCommand();
                    command.CommandText = "INSERT OR IGNORE INTO RoomTypesTours (ToursId, RoomTypesId, CurrentQuantity) VALUES (@toursId, @roomTypesId, @currentQuantity)";

                    command.Parameters.AddWithValue("@toursId", tourId);
                    command.Parameters.AddWithValue("@roomTypesId", roomType.Id);
                    command.Parameters.AddWithValue("@currentQuantity", roomType.DefaultQuantity);

                    command.ExecuteNonQuery();
                }
            }

            _connection.Close();
        }

        private void AddBookingStates()
        {
            /*
            Adds the default booking states to the database. The default booking states are "Booked", "Refunded" and "Cancelled".
            */

            string[] bookingStates =
            {
                "Booked",
                "Refunded",
                "Cancelled"
            };

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            foreach (string bookingState in bookingStates)
            {
                SqliteCommand command = _connection.CreateCommand();
                command.CommandText = "INSERT OR IGNORE INTO BookingStates (BookingState) VALUES (@bookingState);";

                command.Parameters.AddWithValue("@bookingState", bookingState);

                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public int GetBookingStateId(BookingStatesEnum bookingState)
        {
            /*
            Gets the BookingStatesId for a given BookingStatesEnum.

            Parameters:
                bookingState (BookingStatesEnum): The BookingStatesEnum to get the BookingStatesId for.
            Returns:
                int: The BookingStatesId for the given BookingStatesEnum.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT BookingStatesId FROM BookingStates WHERE BookingState = @bookingState";

            command.Parameters.AddWithValue("@bookingState", bookingState.ToString());

            object? idObject = command.ExecuteScalar();

            _connection.Close();

            if (idObject is null)
            {
                return -1;
            }

            int id = Int32.Parse(idObject.ToString() ?? "");

            return id;
        }

        private void AddDefaultData()
        {
            /*
            Adds the default data to the database.
            */
            
            AddUserRoles();
            AddDefaultAdminUser();
            AddDefaultCustomerUser();
            AddDefaultTourDates();
            AddDefaultRoomTypes();
            SetUpRoomCostInfo();
            SetUpRoomTypesToursInfo();
            AddBookingStates();
        }

        private bool UserExists(string username)
        {
            /*
            Checks if a user exists in the database.

            Parameters:
                username (string): The username to check if exists.
            Returns:
                bool: True if the user exists, False if the user does not exist.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int userId = GetUsersId(username);

            if (userId == -1)
            {
                return false;
            }

            int userCredentialsId = GetUserCredentialsId(userId);

            return userCredentialsId != -1;
        }

        private string GetUserHash(string username)
        {
            /*
            Gets the hashed password for a given username.

            Parameters:
                username (string): The username to get the hashed password for.
            Returns:
                string: The hashed password for the given username.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int userId = GetUsersId(username);

            if (userId == -1)
            {
                return String.Empty;
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT HashedPassword FROM UserCredentials WHERE UsersId = @userId;";

            command.Parameters.AddWithValue("@userId", userId);

            object? hashedObj = command.ExecuteScalar();
            _connection.Close();

            if (hashedObj is null)
            {
                return String.Empty;
            }

            string hashedPassword = hashedObj.ToString() ?? "";

            return hashedPassword;
        }

        private bool MatchingHash(string username, string rawPassword)
        {
            /*
            Checks if a given raw password matches the hashed password for a given username.

            Parameters:
                username (string): The username to check the password for.
                rawPassword (string): The raw password to check.
            Returns:
                bool: True if the raw password matches the hashed password, False if the raw password does not match the hashed password.
            */
            
            string userHash = GetUserHash(username);
            string hashedPassword = UsersHelper.HashPassword(rawPassword);

            return userHash == hashedPassword;
        }

        private bool UserIsAdmin(string username)
        {
            /*
            Checks if a user is an admin.

            Parameters:
                username (string): The username to check if is an admin.
            Returns:
                bool: True if the user is an admin, False if the user is not an admin.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int adminId = GetUserRoleId(UserRolesEnum.Admin);

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT UsersId FROM Users WHERE UserRolesId = @userRolesId AND UsernameOrEmail = @username";

            command.Parameters.AddWithValue("@userRolesId", adminId);
            command.Parameters.AddWithValue("@username", username);

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                _connection.Close();
                return true;
            }
            _connection.Close();
            return false;
        }

        public ResponseModel ValidateUser(string username, string rawPassword)
        {
            /*
            Validates a user.

            Parameters:
                username (string): The username to validate.
                rawPassword (string): The raw password to validate.
            Returns:
                ResponseModel: A ResponseModel object.
            */

            if (!UserExists(username))
            {
                return new ResponseModel(false, "User does not exist");
            }

            if (!MatchingHash(username, rawPassword))
            {
                return new ResponseModel(false, "Password is Incorrect");
            }

            if (!UserIsAdmin(username))
            {
                return new ResponseModel(false, "Only admin users can log in.");
            }

            return new ResponseModel(true, "");
        }

        public int GetNumberOfRoomsAvailableByTourId(int tourId)
        {
            /*
            Gets the number of rooms available for a given tour id.

            Parameters:
                tourId (int): The tour id to get the number of rooms available for.
            Returns:
                int: The number of rooms available for the given tour id.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT CurrentQuantity FROM RoomTypesTours WHERE ToursId = @tourId";

            command.Parameters.AddWithValue("@tourId", tourId);

            int numberOfRooms = 0;

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int currentQuantity = reader.GetInt32(0);
                numberOfRooms += currentQuantity;
            }

            _connection.Close();


            return numberOfRooms;
        }

        private void UpdateEmailAddressUsername(int userId, string newValue)
        {
            /*
            Updates the email address or username for a given user id.

            Parameters:
                userId (int): The user id to update the email address or username for.
                newValue (string): The new value to update the email address or username to.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            StringBuilder sb = new();

            sb.AppendLine("UPDATE Users");
            sb.AppendLine("SET UsernameOrEmail = @newValue");
            sb.AppendLine("WHERE UsersId = @userId");

            command.CommandText = sb.ToString();

            command.Parameters.AddWithValue("@newValue", newValue);
            command.Parameters.AddWithValue("@userId", userId);

            command.ExecuteNonQuery();
            _connection.Close();
        }

        private void UpdateContactDetailsNonNullTelephone(int userId, string fullName, string telephone)
        {
            /*
            Updates the contact details for a given user id.

            Parameters:
                userId (int): The user id to update the contact details for.
                fullName (string): The full name to update the contact details to.
                telephone (string): The telephone to update the contact details to.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            StringBuilder sb = new();

            sb.AppendLine("UPDATE UserContactDetails");
            sb.AppendLine("SET FullName = @fullName,");
            sb.AppendLine("Telephone = @telephone");
            sb.AppendLine("WHERE UsersId = @userId");

            command.CommandText = sb.ToString();

            command.Parameters.AddWithValue("@fullName", fullName);
            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@userId", userId);

            command.ExecuteNonQuery();
            _connection.Close();
        }

        private void UpdateContactDetailsNullTelephone(int userId, string fullName)
        {
            /*
            Updates the contact details for a given user id.

            Parameters:
                userId (int): The user id to update the contact details for.
                fullName (string): The full name to update the contact details to.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            StringBuilder sb = new();

            sb.AppendLine("UPDATE UserContactDetails");
            sb.AppendLine("SET FullName = @fullName,");
            sb.AppendLine("Telephone = null");
            sb.AppendLine("WHERE UsersId = @userId");

            command.CommandText = sb.ToString();

            command.Parameters.AddWithValue("@fullName", fullName);
            command.Parameters.AddWithValue("@userId", userId);

            command.ExecuteNonQuery();
            _connection.Close();
        }

        public void UpdateBookingInfo(AmendUserModel model)
        {
            /*
            Updates the booking info for a given AmendUserModel.

            Parameters:
                model (AmendUserModel): The AmendUserModel to update the booking info for.
            */
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            UpdateEmailAddressUsername(model.UsersID, model.Email);
            UpdateContactDetailsNonNullTelephone(model.UsersID, model.FullName, model.Telephone);
        }

        public void UpdateBookingInfoNullTelephone(AmendUserModel model)
        {
            /*
            Updates the booking info for a given AmendUserModel with a null telephone.

            Parameters:
                model (AmendUserModel): The AmendUserModel to update the booking info for.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            UpdateEmailAddressUsername(model.UsersID, model.Email);
            UpdateContactDetailsNullTelephone(model.UsersID, model.FullName);
        }

        private void UpdateCurrentQuantityByBookingId(int bookingId, int amount = 1)
        {
            /*
            Updates the current quantity by booking id.

            Parameters:
                bookingId (int): The booking id to update the current quantity by.
                amount (int): The amount to update the current quantity by. Default is 1.

            Returns:
                int: The number of rooms available for the given tour id.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            StringBuilder sb = new();

            sb.AppendLine("UPDATE RoomTypesTours");
            sb.AppendLine("SET CurrentQuantity = CurrentQuantity + @amount");
            sb.AppendLine("WHERE ToursId = (SELECT ToursId FROM Bookings WHERE BookingsId = @bookingId)");
            sb.AppendLine("AND RoomTypesId = (SELECT RoomTypesId FROM Bookings WHERE BookingsId = @bookingId);");

            command.CommandText = sb.ToString();

            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@bookingId", bookingId);

            command.ExecuteNonQuery();

            _connection.Close();
        }

        public void UpdateBooking(int bookingId, BookingStatesEnum newStatus)
        {
            /*
            Updates a booking to a new status.

            Parameters:
                bookingId (int): The booking id to update.
                newStatus (BookingStatesEnum): The new status to update the booking to.
            */

            if (newStatus != BookingStatesEnum.Refunded && newStatus != BookingStatesEnum.Cancelled)
            {
                throw new InvalidOperationException("This method can only update to refunded or cancelled");
            }


            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int bookingStatusId = GetBookingStateId(newStatus);

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();

            if (newStatus == BookingStatesEnum.Refunded)
            {
                command.CommandText = "UPDATE Bookings SET BookingPrice = BookingPrice * 0.5, BookingStatesId = @stateId WHERE BookingsId = @bookingId";
            }
            else if (newStatus == BookingStatesEnum.Cancelled)
            {
                command.CommandText = "UPDATE Bookings SET BookingStatesId = @stateId WHERE BookingsId = @bookingId";
            }

            command.Parameters.AddWithValue("@stateId", bookingStatusId);
            command.Parameters.AddWithValue("@bookingId", bookingId);

            command.ExecuteNonQuery();

            _connection.Close();
            UpdateCurrentQuantityByBookingId(bookingId, 1);

        }

        private int GetTotalIncomeGenerateByTour(int tourId)
        {
            /*
            Gets the total income generated by a tour.

            Parameters:
                tourId (int): The tour id to get the total income generated by.
            Returns:
                int: The total income generated by the tour.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT BookingPrice FROM Bookings WHERE ToursId = @tourId;";

            command.Parameters.AddWithValue("@tourId", tourId);

            int generatedIncomePence = 0;

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int bookingPrice = reader.GetInt32(0);
                generatedIncomePence += bookingPrice;
            }

            _connection.Close();


            return generatedIncomePence;
        }

        public int GetUserIdByBookingId(int bookingId)
        {
            /*
            Gets the user id by booking id.

            Parameters:
                bookingId (int): The booking id to get the user id by.
            Returns:
                int: The user id for the given booking id.
            */

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT UsersId FROM Bookings WHERE BookingsId = @bookingId";

            command.Parameters.AddWithValue("@bookingId", bookingId);

            object? userIdObj = command.ExecuteScalar();

            _connection.Close();

            if (userIdObj is null)
            {
                return -1;
            }

            int userId = Int32.Parse(userIdObj.ToString() ?? "");

            return userId;
        }

        public List<ViewBookingModel> GetAllBookingsByTourId(int tourId)
        {
            /*
            Gets all bookings by tour id.

            Parameters:
                tourId (int): The tour id to get all bookings by.
            Returns:
                List<ViewBookingModel>: A list of ViewBookingModel objects.
            */
            List<ViewBookingModel> list = new();

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int activeBookingsId = GetBookingStateId(BookingStatesEnum.Booked);

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            StringBuilder query = new();
            query.AppendLine("SELECT");
            query.AppendLine("Bookings.BookingsId,");
            query.AppendLine("Users.UsernameOrEmail,");
            query.AppendLine("UserContactDetails.FullName,");
            query.AppendLine("UserContactDetails.Telephone,");
            query.AppendLine("Bookings.BookingPrice,");
            query.AppendLine("Tours.TourStartDate,");
            query.AppendLine("RoomTypes.RoomName");
            query.AppendLine("FROM");
            query.AppendLine("Users");
            query.AppendLine("JOIN");
            query.AppendLine("UserContactDetails ON Users.UsersId = UserContactDetails.UsersId");
            query.AppendLine("JOIN");
            query.AppendLine("Bookings ON Users.UsersId = Bookings.UsersId");
            query.AppendLine("JOIN");
            query.AppendLine("Tours ON Bookings.ToursId = Tours.ToursId");
            query.AppendLine("JOIN");
            query.AppendLine("RoomTypes ON Bookings.RoomTypesId = RoomTypes.RoomTypesId");
            query.AppendLine("WHERE");
            query.AppendLine("Tours.ToursId = @TourId");
            query.AppendLine("AND");
            query.AppendLine("Bookings.BookingStatesId = @bookingState;");

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = query.ToString();

            command.Parameters.AddWithValue("@TourId", tourId);
            command.Parameters.AddWithValue("@bookingState", activeBookingsId);

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int bookingsId = reader.GetInt32(0);
                string email = reader.GetString(1);
                string fullName = reader.GetString(2);

                string telephone;
                if (!reader.IsDBNull(3))
                {
                    telephone = reader.GetString(3);
                }
                else
                {
                    telephone = "N/a";
                }

                int bookingPrice = reader.GetInt32(4);
                DateTime tourStartDate = reader.GetDateTime(5);
                string roomName = reader.GetString(6);

                ViewBookingModel viewBooking = new(bookingsId, email, fullName, telephone, bookingPrice, tourStartDate, roomName);

                list.Add(viewBooking);
            }


            _connection.Close();

            return list;
        }

        public DateTime GetDateOfTourByBookingId(int bookingId)
        {
            /*
            Gets the date of a tour by booking id.

            Parameters:
                bookingId (int): The booking id to get the date of the tour by.
            Returns:
                DateTime: The date of the tour for the given booking id.
            */
            
            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT T.TourStartDate FROM Tours T JOIN Bookings B ON B.ToursId = T.ToursId WHERE B.BookingsId = @bookingId LIMIT 1";

            command.Parameters.AddWithValue("@bookingId", bookingId);

            SqliteDataReader reader = command.ExecuteReader();

            DateTime tourDate = DateTime.MinValue;

            if (reader.Read())
            {
                tourDate = reader.GetDateTime(0);
            }

            _connection.Close();
            return tourDate;

        }

        public List<ViewBookingModel> GetAllBookings()
        {
            /*
            Gets all bookings.

            Returns:
                List<ViewBookingModel>: A list of ViewBookingModel objects.
            */
            
            List<ViewBookingModel> list = new();

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            int activeBookingsId = GetBookingStateId(BookingStatesEnum.Booked);

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            StringBuilder query = new();
            query.AppendLine("SELECT");
            query.AppendLine("Bookings.BookingsId,");
            query.AppendLine("Users.UsernameOrEmail,");
            query.AppendLine("UserContactDetails.FullName,");
            query.AppendLine("UserContactDetails.Telephone,");
            query.AppendLine("Bookings.BookingPrice,");
            query.AppendLine("Tours.TourStartDate,");
            query.AppendLine("RoomTypes.RoomName");
            query.AppendLine("FROM");
            query.AppendLine("Users");
            query.AppendLine("JOIN");
            query.AppendLine("UserContactDetails ON Users.UsersId = UserContactDetails.UsersId");
            query.AppendLine("JOIN");
            query.AppendLine("Bookings ON Users.UsersId = Bookings.UsersId");
            query.AppendLine("JOIN");
            query.AppendLine("Tours ON Bookings.ToursId = Tours.ToursId");
            query.AppendLine("JOIN");
            query.AppendLine("RoomTypes ON Bookings.RoomTypesId = RoomTypes.RoomTypesId");
            query.AppendLine("WHERE");
            query.AppendLine("Bookings.BookingStatesId = @bookingStateId;");

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = query.ToString();
            command.Parameters.AddWithValue("@bookingStateId", activeBookingsId);


            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int bookingsId = reader.GetInt32(0);
                string email = reader.GetString(1);
                string fullName = reader.GetString(2);

                string telephone;
                if (!reader.IsDBNull(3))
                {
                    telephone = reader.GetString(3);
                }
                else
                {
                    telephone = "N/a";
                }

                int bookingPrice = reader.GetInt32(4);
                DateTime tourStartDate = reader.GetDateTime(5);
                string roomName = reader.GetString(6);

                ViewBookingModel viewBooking = new(bookingsId, email, fullName, telephone, bookingPrice, tourStartDate, roomName);

                list.Add(viewBooking);
            }

            _connection.Close();

            return list;
        }

        public List<ViewTourModel> GetViewTourModels()
        {
            /*
            Gets all ViewTourModel objects.

            Returns:
                List<ViewTourModel>: A list of ViewTourModel objects.
            */
            
            List<ViewTourModel> list = new();

            // No Connection Available
            if (_connection == null)
            {
                throw new InvalidOperationException("Database Connection Not Set Up");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            List<TourWithIdModel> basicTourDetails = GetAllTourDatesWithIds();

            foreach (TourWithIdModel tourWithId in basicTourDetails)
            {
                int totalIncome = GetTotalIncomeGenerateByTour(tourWithId.Id);
                int numberOfRoomsAvailable = GetNumberOfRoomsAvailableByTourId(tourWithId.Id);


                ViewTourModel viewTour = new(totalIncome, numberOfRoomsAvailable, tourWithId.Id, tourWithId.TourStarDate);

                list.Add(viewTour);
            }

            return list;
        }

        public void Dispose()
        {
            /*
            Disposes the connection to the database.
            */
            
            if (_connection != null)
            {
                Console.WriteLine("Disposing connection");
                _connection.Close();
                _connection.Dispose();
            }
            else
            {
                Console.WriteLine("Connection was already null");
            }
        }
    }


}
