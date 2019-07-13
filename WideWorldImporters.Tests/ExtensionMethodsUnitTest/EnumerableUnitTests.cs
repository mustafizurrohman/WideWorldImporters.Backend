using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WideWorldImporters.Core.ExtensionMethods;
using Xunit;

namespace WideWorldImporters.Tests.ExtensionMethodsUnitTest
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumerableUnitTests
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        [Theory]
        [InlineData(100)]
        public void TestShuffle(int number)
        {
            var originalList = Enumerable.Range(0, number).ToList();
            var shuffledList = originalList.Shuffle();

            var diff = originalList.Except(shuffledList).ToList();

            Assert.True(diff.IsEmpty());

        }
    }
}
