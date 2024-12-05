using OOPA2.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OOPA2.Helpers
{
    public static class UsersHelper
    {
        public static string LoggedInUsername { get; set; } = "";
        public static int SelectedTourId { get; set; } = -1;
        public static int UserToAmendId { get; set; } = -1;

        private static ResponseModel IsAlphanumeric(string s)
        {
            /*
            Checks if a string is alphanumeric

            Parameters:
                s (string): The string to check
            Returns:
                ResponseModel: A response model with the result of the check
            */

            ResponseModel responseModel = new(Regex.IsMatch(s, "^[a-zA-Z0-9]+$"), "");

            if (!responseModel.IsSuccess)
            {
                responseModel.Reason = "Provided string is not alphanumeric";
            }

            return responseModel;
        }

        public static ResponseModel IsValidUsername(string username)
        {
            /*
            Checks if a username is valid

            Parameters:
                username (string): The username to check
            Returns:
                ResponseModel: A response model with the result of the check
            */

            // Null Check
            if (username is null)
            {
                return new ResponseModel(false, "Username should not be null");
            }

            int usernameLength = username.Length;

            if (usernameLength < 5)
            {
                return new ResponseModel(false, "Username should be at least 5 characters long");
            }

            if (usernameLength > 20)
            {
                return new ResponseModel(false, "Username should be less than 20 characters long");
            }

            return IsAlphanumeric(username);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "<Pending>")]
        private static ResponseModel PasswordCharacterValidator(string password)
        {
            /*
            Validates the characters in a password

            Parameters:
                password (string): The password to validate
            Returns:
                ResponseModel: A response model with the result of the check
            */

            // Passwords shouldn't contain spaces
            if (password.Contains(' '))
            {
                return new ResponseModel(false, "Password Should Not Contain Spaces");
            }


            // Should be mix case
            string allLowerCase = password.ToLower();

            if (password == allLowerCase)
            {
                return new ResponseModel(false, "Password Should Contain At Least One Uppercase Letter");
            }

            string allUpperCase = password.ToUpper();

            if (password == allUpperCase)
            {
                return new ResponseModel(false, "Password Should Contain At Least One Lowercase Letter");
            }

            // Contain At Least One Number
            bool containsOneOrMoreDigits = Regex.IsMatch(password, @"\d{1,}");

            if (!containsOneOrMoreDigits)
            {
                return new ResponseModel(false, "Password Should Contain At Least One Digit");
            }

            // Contain a Special Charachter
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

            bool containsSpecialChar = false;

            foreach (char specialChar in specialChars)
            {
                if (password.Contains(specialChar))
                {
                    containsSpecialChar = true;
                    break;
                }
            }

            if (!containsSpecialChar)
            {
                string specialCharString = new(specialChars);

                return new ResponseModel(false, $"Password must contain at least one of {specialCharString}");
            }

            return new ResponseModel(true, "");
        }

        public static ResponseModel IsValidPassword(string password)
        {
            /*
            Checks if a password is valid

            Parameters:
                password (string): The password to check
            Returns:
                ResponseModel: A response model with the result of the check
            */

            // Null Check
            if (password is null)
            {
                return new ResponseModel(false, "Password should not be null");
            }

            int passwordLength = password.Length;

            if (passwordLength < 8)
            {
                return new ResponseModel(false, "Password should be at least 8 characters long");
            }

            ResponseModel passwordCharacterModel = PasswordCharacterValidator(password);

            return passwordCharacterModel;
        }

        public static string HashPassword(string plainText)
        {
            /*
            Hashes a password using SHA256

            Parameters:
                plainText (string): The password to hash
            Returns:
                string: The hashed password
            */

            /*
             Based on code from https://stackoverflow.com/questions/12416249/hashing-a-string-with-sha256
             */
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                StringBuilder sb = new();

                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public static ResponseModel IsValidTelephone(string telephone)
        {
            /*
            Checks if a telephone number is valid

            Parameters:
                telephone (string): The telephone number to check
            Returns:
                ResponseModel: A response model with the result of the check
            */

            // This would allow for some false positives
            // But should be allowable in a prototype

            // Check is not null
            if (telephone is null)
            {
                return new ResponseModel(false, "Telephone number cannot be null");
            }

            // Remove any spaces/dashes
            telephone = telephone.Trim();
            telephone = telephone.Replace('+', ' ');
            telephone = telephone.Replace('-', ' ');
            telephone = telephone.Replace(" ", "");

            // Length check

            // Minimum length should be 3 (111, 999, 101)
            if (telephone.Length < 3)
            {
                return new ResponseModel(false, "Telephone number must be at least 3 digits");
            }

            // Maximum length should be 13
            if (telephone.Length > 13)
            {
                return new ResponseModel(false, "Telephone number must be no more than 13 digits");
            }

            bool isAllDigits = Regex.IsMatch(telephone, @"^\d{3,13}$");

            if (!isAllDigits)
            {
                return new ResponseModel(false, "Telephone number must only contain digits");
            }

            return new ResponseModel(true, "");
        }

        private static int CountOccurences(string s, char c)
        {
            /*
            Counts the number of times a character appears in a string

            Parameters:
                s (string): The string to check
                c (char): The character to count
            Returns:
                int: The number of times the character appears in the string
            */
            
            int count = 0;

            if (!s.Contains(c))
            {
                return count;
            }

            foreach (char ch in s)
            {
                if (ch == c)
                {
                    count++;
                }
            }

            return count;
        }

        public static ResponseModel IsValidEmail(string email)
        {
            /*
            Checks if an email is valid

            Parameters:
                email (string): The email to check
            Returns:
                ResponseModel: A response model with the result of the check
            */
            
            // The only true way to validate an email
            // Is to attempt to send an email
            // So for this prototype, the system checks
            // for the presenece of @ and at least one .

            if (email is null)
            {
                return new ResponseModel(false, "Email cannot be null");
            }

            // a@b.c
            if (email.Length < 5)
            {
                return new ResponseModel(false, "Email must be at least 5 characters in length.");
            }

            if (!email.Contains('@'))
            {
                return new ResponseModel(false, "Email must contain an @");
            }

            if (CountOccurences(email, '@') != 1)
            {
                return new ResponseModel(false, "Email can only contain one @");
            }

            if (!email.Contains('.'))
            {
                return new ResponseModel(false, "Email must contain at least one .");
            }

            // make sure theres a . after the @
            string[] emailParts = email.Split('@');
            string domain = emailParts[^1];

            if (!domain.Contains('.'))
            {
                return new ResponseModel(false, "The domain of an email must contain at least one .");
            }


            return new ResponseModel(true, "");
        }

        private static ResponseModel LuhnAlgorithm(string cardNumber)
        {
            /*
            Checks if a credit card number is valid using the Luhn Algorithm

            Parameters:
                cardNumber (string): The credit card number to check
            Returns:
                ResponseModel: A response model with the result of the check
            */

            // Reverse the card number and remove any non-digit characters
            string reversedCardNumber = new string(cardNumber.Where(char.IsDigit).Reverse().ToArray());

            int digitSum = 0;
            bool isAlternate = false;  // This tracks whether to double the value or not.

            foreach (char digit in reversedCardNumber)
            {
                if (int.TryParse(digit.ToString(), out int value))
                {
                    if (isAlternate)
                    {
                        value *= 2;
                        if (value > 9)
                        {
                            value = value / 10 + value % 10; // Sum the digits of the value
                        }
                    }

                    digitSum += value;
                    isAlternate = !isAlternate; // Flip the isAlternate flag
                }
            }

            bool isValid = (digitSum % 10) == 0;
            return new ResponseModel(isValid, isValid ? "" : "Invalid Credit Card Number");
        }

        public static ResponseModel IsValidCreditCardNumber(string cardNumber)
        {
            /*
            Checks if a credit card number is valid

            Parameters:
                cardNumber (string): The credit card number to check
            Returns:
                ResponseModel: A response model with the result of the check
            */

            if (cardNumber is null)
            {
                return new ResponseModel(false, "Card Number cannot be null");
            }

            cardNumber = cardNumber.Trim();
            cardNumber = cardNumber.Replace(" ", "");

            if (cardNumber.Length < 13)
            {
                return new ResponseModel(false, "Card Number must be 13 digits or longer");
            }

            if (cardNumber.Length > 19)
            {
                return new ResponseModel(false, "Card Number must be 19 digits or shorter");
            }

            bool isAllDigits = Regex.IsMatch(cardNumber, @"^\d{1,}$");

            if (!isAllDigits)
            {
                return new ResponseModel(false, "Card number must only contain digits");
            }

            return LuhnAlgorithm(cardNumber);
        }
    }
}
