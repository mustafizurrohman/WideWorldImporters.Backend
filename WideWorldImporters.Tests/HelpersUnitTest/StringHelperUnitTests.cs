using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Core.Helpers;
using Xunit;

namespace WideWorldImporters.Tests.HelpersUnitTest
{
    /// <summary>
    /// Unit test for String helpers
    /// </summary>
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class StringHelperUnitTests
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfTests"></param>
        [Theory]
        [InlineData(10000)]
        public void TestIfGeneratedStingsAreRandom(int numberOfTests)
        {
            var randomStrings = Enumerable.Range(0, numberOfTests)
                .Select(x => StringHelpers.GetRandomString(10))
                .Distinct()
                .ToList();

            randomStrings.Aggregate((a, b) => a + " " + b);

            // Assuming that 10% may be duplicates 
            Assert.True(randomStrings.Count > numberOfTests * 0.9) ;

        }

        // Stopwatch restart
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfTests"></param>
        /// <param name="passwordLength"></param>
        [Theory]
        [InlineData(5000, 16)]
        public void TestPasswordGeneration(int numberOfTests, int passwordLength)
        {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();

            var passwords = Enumerable.Range(0, numberOfTests)
                .Select(x => StringHelpers.GetRandomPassword(passwordLength))
                .ToList();
            var time = stopwatch1.ElapsedMilliseconds;

            stopwatch1.Restart();
            var allPasswordsAggr = passwords.Aggregate((a, b) => a + Environment.NewLine + b);
            var time2 = stopwatch1.ElapsedMilliseconds;

            stopwatch1.Restart();
            StringBuilder allPasswords = new StringBuilder();
            foreach (string password in passwords)
            {
                allPasswords.Append(password + Environment.NewLine);
            }

            stopwatch1.Stop();
            var time3 = stopwatch1.ElapsedMilliseconds;

            // Lol
            Assert.True(time3 < time2);

            var allPasswordsStringLength = allPasswords.ToString().Length;

            foreach (var password in passwords)
            {
                Assert.True(password.IsValidPassword());
                Assert.True(password.RemoveDuplicates().Length == password.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void TestReplace()
        {
            var string1 = "abcdcddum__292";
            var string1Res = StringHelpers.ReplaceDuplicateCharacters(string1);

            Assert.True(string1 != string1Res);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void TestDuplicateRemoval()
        {
            var string1 = "abcdcddum__292";
            var string1Res = string1.RemoveDuplicates();

            Assert.True(string1Res == "abcdum_29");
        }

    }

}
