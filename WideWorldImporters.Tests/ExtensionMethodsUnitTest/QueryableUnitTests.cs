using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WideWorldImporters.Core.ExtensionMethods;
using Xunit;

namespace WideWorldImporters.Tests.ExtensionMethodsUnitTest
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryableUnitTests
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        [Theory]
        [InlineData(100)]
        public void TestSmartTakeFalse(int number)
        {
            var originalList = Enumerable.Range(0, number).AsQueryable();

            var take = originalList.SmartTake(number + 2);

            Assert.False(take.Item2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        [Theory]
        [InlineData(100)]
        public void TestSmartTakeTrue(int number)
        {
            var originalList = Enumerable.Range(0, number).AsQueryable();

            var take = originalList.SmartTake(number - 2);

            Assert.True(take.Item2);
        }

    }
}
