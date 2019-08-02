using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace WideWorldImporters.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for string
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Returns true of the provided string represents an email. Otherwise false.
        /// Does not actually verify the email. 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true of the provided string represents an email. Otherwise false.
        /// Does not actually verify the email. 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(this string password)
        {
            if (password.Length < 8 || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            bool containsUppercase = password.Any(char.IsUpper);
            bool containsLowercase = password.Any(char.IsLower);
            bool containsDigit = password.Any(char.IsDigit);
            bool containsSpecialChars = password.ContainsSpecialCharacters();
                
            return containsUppercase && containsLowercase && containsDigit && containsSpecialChars;
        }

        /// <summary>
        /// Returns true of the provided string represents an email. Otherwise false.
        /// Does not actually verify the email. 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ContainsSpecialCharacters(this string password)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            var regularExpression = new Regex(specialChar);
            bool containsSpecialChar = regularExpression.IsMatch(password);

            return containsSpecialChar;
        }

    }
}
