using Microsoft.Data.Sqlite;
using NUnit.Framework;
using OOPA2.Database;
using OOPA2.Enums;
using OOPA2.Models;
using System.Runtime.InteropServices;

namespace OOPA2Test
{
    [TestFixture]
    public class DatabaseControllerTests
    {
        private DatabaseController _dbController;
        private string _dbPath;

        // https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-movefileexa
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, int dwFlags);
        const int MOVEFILE_DELAY_UNTIL_REBOOT = 0x4;

        [SetUp]
        public void Setup()
        {
            _dbPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".sqlite");
            _dbController = new DatabaseController(_dbPath);
        }

        [Test]
        public void Database_Should_Exist()
        {
            // Test to ensure the database file is created
            Assert.That(File.Exists(_dbPath), Is.True);
        }

        [Test]
        public void AdminRole_ShouldHaveId_GreaterThan_0()
        {
            UserRolesEnum adminRole = UserRolesEnum.Admin;
            int adminId = _dbController.GetUserRoleId(adminRole);

            Assert.That(adminId, Is.GreaterThan(0));
        }

        [Test]
        public void CustomerRole_ShouldHaveId_GreaterThan_Admin()
        {
            UserRolesEnum adminRole = UserRolesEnum.Admin;
            int adminId = _dbController.GetUserRoleId(adminRole);

            UserRolesEnum customerRole = UserRolesEnum.Customer;
            int customerId = _dbController.GetUserRoleId(customerRole);

            Assert.That(customerId, Is.GreaterThan(adminId));
        }

        [Test]
        public void ValidateUser_NonExistantUser_ShouldFail()
        {
            ResponseModel model = _dbController.ValidateUser("ThisIsAFakeUsername", "ThisPassword");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("User does not exist"));
            });
        }

        [Test]
        public void ValidateUser_IncorrectPassword_ShouldFail()
        {
            ResponseModel model = _dbController.ValidateUser("DefaultAdmin", "ThisPassword");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Password is Incorrect"));
            });
        }

        [Test]
        public void ValidateUser_NoneAdminUser_ShouldFail()
        {

            _dbController.AddUser("customer", "password", UserRolesEnum.Customer);

            ResponseModel model = _dbController.ValidateUser("customer", "password");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Only admin users can log in."));
            });
        }

        [Test]
        public void ValidateUser_ValidAdmin_ShouldPass()
        {

            ResponseModel model = _dbController.ValidateUser("DefaultAdmin", "DefaultAdminPass123!");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.True);
                Assert.That(model.Reason, Is.EqualTo(""));
            });
        }

        [Test]
        public void AddUser_ShouldBeSuccessful()
        {
            _dbController.AddUser("TestUser", "TestPassword", UserRolesEnum.Admin);

            int userId = _dbController.GetUsersId("TestUser");

            Assert.That(userId, Is.GreaterThan(0));
        }

        [Test]
        public void AddUserContactDetails_WithoutTelephone_ShouldBeSuccessful()
        {
            _dbController.AddUser("TestUser", "TestPassword", UserRolesEnum.Customer);

            int userId = _dbController.GetUsersId("TestUser");

            Assert.That(userId, Is.GreaterThan(0));
            Assert.DoesNotThrow(() =>
            {
                _dbController.AddUserContactDetails(userId, "Test User");
            });
        }

        [Test]
        public void AddUserContactDetails_WithTelephone_ShouldBeSuccessful()
        {
            _dbController.AddUser("TestUser", "TestPassword", UserRolesEnum.Customer);

            int userId = _dbController.GetUsersId("TestUser");

            Assert.That(userId, Is.GreaterThan(0));
            Assert.DoesNotThrow(() =>
            {
                _dbController.AddUserContactDetails(userId, "Test User", "0800001066");
            });
        }

        [Test]
        public void GetBookingStateId_ShouldBeSuccessful()
        {
            int bookingStateId = _dbController.GetBookingStateId(BookingStatesEnum.Booked);

            Assert.That(bookingStateId, Is.GreaterThan(0));
            Assert.DoesNotThrow(() =>
            {
                _dbController.GetBookingStateId(BookingStatesEnum.Booked);
            });
        }

        [Test]
        public void ManageBooking_ReducesNumberOfAvailable_Rooms()
        {
            int original = _dbController.GetNumberOfRoomsAvailableByTourId(1);

            _dbController.ManageBooking(1, 1, 1);

            int updated = _dbController.GetNumberOfRoomsAvailableByTourId(1);

            Assert.That(updated, Is.LessThan(original));
            Assert.That(updated, Is.EqualTo(original - 1));
        }

        [Test]
        public void GetAllTourDatesWithIds_ListShouldNotBeEmpty()
        {
            List<TourWithIdModel> data = _dbController.GetAllTourDatesWithIds();

            Assert.That(data, Is.Not.Empty);
        }

        [Test]
        public void GetAvailableRoomTypesByTour_ListShouldNotBeEmpty()
        {
            List<RoomTypeWithIdAndQuantityModel> data = _dbController.GetAvailableRoomTypesByTour(1);

            Assert.That(data, Is.Not.Empty);
        }

        [Test]
        public void GetRoomDefaultPriceByRoomTypesToursId_ValidRTTId_GreaterThanMinusOne()
        {
            int defaultPrice = _dbController.GetRoomDefaultPriceByRoomTypesToursId(1);

            Assert.That(defaultPrice, Is.GreaterThan(-1));
        }

        [Test]
        public void GetRoomDefaultPriceByRoomTypesToursId_InvalidRTTId_EqualToMinusOne()
        {
            int defaultPrice = _dbController.GetRoomDefaultPriceByRoomTypesToursId(-1);

            Assert.That(defaultPrice, Is.EqualTo(-1));
        }


        [Test]
        public void GetRoomDiscountRateByRoomTypesToursId_ValidRTTId_GreaterThanMinusOne()
        {
            int discountRate = _dbController.GetRoomDiscountRateByRoomTypesToursId(1);

            Assert.That(discountRate, Is.GreaterThan(-1));
        }

        [Test]
        public void GetRoomDiscountRateByRoomTypesToursId_InvalidRTTId_EqualToMinusOne()
        {
            int discountRate = _dbController.GetRoomDiscountRateByRoomTypesToursId(-1);

            Assert.That(discountRate, Is.EqualTo(-1));
        }

        [Test]
        public void GetViewTourModels_ShouldNotBeEmpty()
        {
            List<ViewTourModel> models = _dbController.GetViewTourModels();

            Assert.That(models, Is.Not.Empty);
        }

        [Test]
        public void GetAllBookingsByTourId_ShouldBeEmpty()
        {
            List<ViewBookingModel> models = _dbController.GetAllBookingsByTourId(1);

            Assert.That(models, Is.Empty);
        }

        [Test]
        public void GetAllBookings_ShouldBeEmpty()
        {
            List<ViewBookingModel> models = _dbController.GetAllBookings();

            Assert.That(models, Is.Empty);
        }

        [Test]
        public void UpdateBooking_InvalidId_ShouldThrowException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _dbController.UpdateBooking(1, BookingStatesEnum.Booked));
            Assert.That(ex.Message, Is.EqualTo("This method can only update to refunded or cancelled"));
        }

        [TearDown]
        public void Teardown()
        {
            _dbController.Dispose();
            string path = Path.GetTempPath();
            var files = Directory.GetFiles(path); // Assuming GetAllFilesInPath() is intended to get all files in the directory.
            var sqliteFiles = files.Where(f => f.EndsWith(".sqlite")); // Use 'Where' to filter and 'EndsWith' to match file extension.

            foreach (var file in sqliteFiles)
            {
                MoveFileEx(file, null, MOVEFILE_DELAY_UNTIL_REBOOT); // force deletion
            }
        }


    }
}
