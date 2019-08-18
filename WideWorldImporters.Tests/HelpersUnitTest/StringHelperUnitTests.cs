using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfTests"></param>
        /// <param name="passwordLength"></param>
        [Theory]
        [InlineData(1000, 20)]
        public void TestPasswordGeneration(int numberOfTests, int passwordLength)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var passwords = Enumerable.Range(0, numberOfTests)
                .Select(x => StringHelpers.GetRandomPassword(passwordLength))
                .ToList();

            stopwatch.Stop();

            var time = stopwatch.ElapsedMilliseconds;

            var allPasswords = passwords.Aggregate((a, b) => a + Environment.NewLine + b);

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
