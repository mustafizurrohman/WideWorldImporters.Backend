using System;
using System.Collections.Generic;
using System.Linq;
using WideWorldImporters.Core.ExtensionMethods;

namespace WideWorldImporters.Core.Helpers
{

    /// <summary>
    /// Helper functions for strings
    /// </summary>
    public static class StringHelpers
    {

        /// <summary>
        /// Returns a randomString of specified length
        /// </summary>
        /// <param name="length"></param>
        /// <param name="printable"></param>
        /// <returns></returns>
        public static string GetRandomString(int length = 20, bool printable = true)
        {

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "Length must be positive!");
            }

            // char.MinValue, char.MaxValue
            List<char> availableCharacters = new List<char>();


            if (!printable)
            {

                availableCharacters = Enumerable.Range(char.MinValue, char.MaxValue)
                                                .Select(x => (char)x)
                                                .Where(c => !char.IsControl(c))
                                                .ToList();
            }
            else
            {
                availableCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890+-*/!§$%&/()".ToList();
            }

            string generatedString = new string(Enumerable
                                                    .Repeat(availableCharacters, length)
                                                    .Select(s => s[RandomHelpers.Next(s.Count)]
                                               ).ToArray());

            return generatedString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomPassword(int length = 10)
        {
            if (length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters.");
            }

            string password = string.Empty;

            password += CharHelpers.GetRandomLowercaseCharacter();
            password += CharHelpers.GetRandomUppercaseCharacter();
            password += CharHelpers.GetRandomSpecialCharacter();
            password += IntHelpers.GetRandomNumber(0, 9);

            for (int i = 4; i < length; i++)
            {
                password += CharHelpers.GetRandomCharacter();
            }

            password = password.Randomize().Randomize();

            return password;
        }

    }
}
