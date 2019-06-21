using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WideWorldImporters.Core.Helpers;
using Xunit;

namespace WideWorldImporters.Tests.HelpersUnitTest
{
    /// <summary>
    /// Unit-tests for IntHelper
    /// </summary>
    public class IntHelperUnitTests
    {


        /// <summary>
        /// Tests if a random number is generated in the applied 
        /// </summary>
        /// <param name="numberOfTests">Number of tests to perform</param>
        /// <param name="minValue">Min Value</param>
        /// <param name="maxValue">Max Value</param>
        [Theory]
        [InlineData(100, 100, 1000)]
        public void TestRandomNumberInRange(int numberOfTests, int minValue, int maxValue)
        {

            List<int> randomNumbers = Enumerable.Range(0, numberOfTests)
                .Select(x => IntHelpers.GetRandomNumber(minValue, maxValue))
                .ToList();

            foreach(var randomNumber in randomNumbers)
            {
                Assert.True(randomNumber >= minValue && randomNumber <= maxValue);
            }

            var uniqueNumbers = randomNumbers.Distinct();

            Assert.True(uniqueNumbers.Count() > 1);

        }

    }

}
