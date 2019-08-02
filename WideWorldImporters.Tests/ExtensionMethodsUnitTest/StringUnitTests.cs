using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Core.ExtensionMethods;
using Xunit;
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

    }
}
