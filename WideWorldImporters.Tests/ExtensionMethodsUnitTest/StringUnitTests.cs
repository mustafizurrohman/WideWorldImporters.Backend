using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Internal;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Core.Helpers;
using Xunit;
using StringHelpers = WideWorldImporters.Core.Helpers.StringHelpers;

// ReSharper disable StringLiteralTypo

namespace WideWorldImporters.Tests.ExtensionMethodsUnitTest
{
    /// <summary>
    /// Unit tests for String extension methods
    /// </summary>
    public class StringUnitTests
    {

        /// <summary>
        /// Check if a couple of strings contains special characters or not
        /// </summary>
        [Fact]
        public void TestIfStringHasSpecialCharacters()
        {
            var string1 = "Abcd";

            Assert.False(string1.ContainsSpecialCharacters());

            var string2 = "A_";
            Assert.True(string2.ContainsSpecialCharacters());
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfTests"></param>
        /// <param name="stringLength"></param>
        [Theory]
        [InlineData(100, 10)]
        public void TestStringRandomization(int numberOfTests, int stringLength)
        {
            var originalStringsList = Enumerable.Range(0, numberOfTests)
                .Select(x => StringHelpers.GetRandomString(stringLength))
                .ToList();

            foreach (string originalString in originalStringsList)
            {
                var randomizedString = originalString.Randomize();
                Assert.False(originalString == randomizedString);
            }

        }

    }
}
