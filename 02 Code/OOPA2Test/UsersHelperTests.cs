using OOPA2.Helpers;
using OOPA2.Models;

namespace OOPA2Test
{
    [TestFixture]
    public class UserHelpersTest
    {
       
        [SetUp]
        public void Setup()
        {
            return;
        }

        [Test]
        public void IsValidUsername_WithNullUsername_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidUsername(null);

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Username should not be null"));
            });
        }

        [Test]
        public void IsValidUsername_WithTooShortUsername_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidUsername("abc");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Username should be at least 5 characters long"));
            });
        }

        [Test]
        public void IsValidUsername_WithTooLongUsername_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidUsername("abcderfghijklmnopqwersrtyuhvcdswsxcgytfdxzasd");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Username should be less than 20 characters long"));
            });
        }

        [Test]
        public void IsValidUsername_WithNonAlphanumericUsername_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidUsername("Test User");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Provided string is not alphanumeric"));
            });
        }


        [Test]
        public void IsValidUsername_WithValidUsername_ShouldPass()
        {
            ResponseModel model = UsersHelper.IsValidUsername("TestUser");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.True);
                Assert.That(model.Reason, Is.EqualTo(""));
            });
        }

        [Test]
        public void IsValidPassword_WithNullPassword_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidPassword(null);

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Password should not be null"));
            });
        }

        [Test]
        public void IsValidPassword_WithTooShortPassword_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidPassword("abc");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Password should be at least 8 characters long"));
            });
        }

        [Test]
        public void IsValidPassword_PasswordWithSpace_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidPassword("abcdefgi jklmnopq");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Password Should Not Contain Spaces"));
            });
        }

        [Test]
        public void IsValidPassword_PasswordAllLowercase_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidPassword("abcdefgi");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Password Should Contain At Least One Uppercase Letter"));
            });
        }

        [Test]
        public void IsValidPassword_PasswordAllUppercase_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidPassword("ABCDEFGHI");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Password Should Contain At Least One Lowercase Letter"));
            });
        }

        [Test]
        public void IsValidPassword_NoDigitInPassword_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidPassword("myReallyGoodPassword");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Password Should Contain At Least One Digit"));
            });
        }

        [Test]
        public void IsValidPassword_NoSpecialCharacterInPassword_ShouldFail()
        {
            char[] specialChars =
            {
                '!',
                '£',
                '$',
                '%',
                '&',
                '*',
                '?',
                '@',
                '#'
            };

            string specialCharString = new(specialChars);
            string expectedMessage = $"Password must contain at least one of {specialCharString}";

            ResponseModel model = UsersHelper.IsValidPassword("myReallyGoodPassword123");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo(expectedMessage));
            });
        }

        [Test]
        public void IsValidPassword_WithValidPassword_ShouldPass()
        {
            ResponseModel model = UsersHelper.IsValidPassword("myReallyGoodPassword123!");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.True);
                Assert.That(model.Reason, Is.EqualTo(""));
            });
        }

        [Test]
        public void HashPassword_ShouldBe_Consistent()
        {
            string password = "HelloWorld";
            HashSet<string> hashes = new();

            // Hash 100 times to show it is determenistic
            for(int i = 0; i < 100; i++)
            {
                string hash = UsersHelper.HashPassword(password);
                hashes.Add(hash);
            }

            Assert.That(hashes, Has.Count.EqualTo(1));
        }

        [Test]
        public void IsValidTelephone_NullTelephone_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidTelephone(null);

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Telephone number cannot be null"));
            });
        }

        [Test]
        public void IsValidTelephone_TooShort_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidTelephone("11");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Telephone number must be at least 3 digits"));
            });
        }

        [Test]
        public void IsValidTelephone_TooLong_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidTelephone("123456789123456789");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Telephone number must be no more than 13 digits"));
            });
        }

        [Test]
        public void IsValidTelephone_NotAllDigits_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidTelephone("123456a");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Telephone number must only contain digits"));
            });
        }

        [Test]
        public void IsValidTelephone_ValidTelephone_ShouldPass()
        {
            ResponseModel model = UsersHelper.IsValidTelephone("0800001066");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.True);
                Assert.That(model.Reason, Is.EqualTo(""));
            });
        }

        [Test]
        public void IsValidEmail_NullEmail_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidEmail(null);

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Email cannot be null"));
            });
        }

        [Test]
        public void IsValidEmail_TooShort_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidEmail("abs");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Email must be at least 5 characters in length."));
            });
        }

        [Test]
        public void IsValidEmail_NoAtSign_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidEmail("abcdefgh");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Email must contain an @"));
            });
        }

        [Test]
        public void IsValidEmail_MoreThanOneAt_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidEmail("congr@ul@e");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Email can only contain one @"));
            });
        }

        [Test]
        public void IsValidEmail_NoFullStop_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidEmail("congr@");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Email must contain at least one ."));
            });
        }

        [Test]
        public void IsValidEmail_NoFullStopAfterAt_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidEmail("..congr@");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("The domain of an email must contain at least one ."));
            });
        }

        [Test]
        public void IsValidEmail_ValidEmail_ShouldPass()
        {
            ResponseModel model = UsersHelper.IsValidEmail("hello@mail.me");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.True);
                Assert.That(model.Reason, Is.EqualTo(""));
            });
        }

        [Test]
        public void IsValidCreditCardNumber_NullCreditCardNumber_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidCreditCardNumber(null);

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Card Number cannot be null"));
            });
        }

        [Test]
        public void IsValidCreditCardNumber_TooShort_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidCreditCardNumber("12345");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Card Number must be 13 digits or longer"));
            });
        }

        [Test]
        public void IsValidCreditCardNumber_TooLong_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidCreditCardNumber("123451234551234512345");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Card Number must be 19 digits or shorter"));
            });
        }

        [Test]
        public void IsValidCreditCardNumber_NotAllDigits_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidCreditCardNumber("123451234512ab");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Card number must only contain digits"));
            });
        }

        [Test]
        public void IsValidCreditCardNumber_FailsLuhn_ShouldFail()
        {
            ResponseModel model = UsersHelper.IsValidCreditCardNumber("1234567891011121");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.False);
                Assert.That(model.Reason, Is.EqualTo("Invalid Credit Card Number"));
            });
        }

        [Test]
        public void IsValidCreditCardNumber_ValidNumber_ShouldPass()
        {
            ResponseModel model = UsersHelper.IsValidCreditCardNumber("5454545454545454");

            Assert.Multiple(() =>
            {
                Assert.That(model.IsSuccess, Is.True);
                Assert.That(model.Reason, Is.EqualTo(""));
            });
        }

        [TearDown]
        public void Teardown()
        {
            return;
        }


    }
}
