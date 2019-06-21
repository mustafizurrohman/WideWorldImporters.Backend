using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WideWorldImporters.Core.Helpers;
using Xunit;

namespace WideWorldImporters.Tests.HelpersUnitTest
{
    /// <summary>
    /// Unit test for String helpers
    /// </summary>
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

            var allStrings = randomStrings.Aggregate((a, b) => a + " " + b);

            // Assuming that 10% may be duplicates 
            Assert.True(randomStrings.Count > numberOfTests * 0.9) ;

        }

    }

}
