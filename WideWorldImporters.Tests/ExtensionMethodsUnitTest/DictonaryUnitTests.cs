using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace WideWorldImporters.Tests.ExtensionMethodsUnitTest
{
    /// <summary>
    /// Unit Tests for Dictionary
    /// </summary>
    public class DictonaryUnitTests
    {
        private static readonly Dictionary<string, string> Dictionary = GetDictionary();

        /// <summary>
        /// Test retrival of valid key
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        public void TestValidRetrival(string key)
        {
            var retrivedValue = Dictionary.GetValueOrDefault(key);
            Assert.Equal(key, retrivedValue);
        }

        /// <summary>
        /// Test if retrival of invalid key leads to the return of a default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        [Theory]
        [InlineData("invalid1", "abcd1")]
        [InlineData("invalid2", "abcd2")]
        [InlineData("invalid3", "abcd3")]
        [InlineData("invalid3", null)]
        public void TestInvalidRetrival(string key, string defaultValue)
        {
            var retrivedValue2 = Dictionary.GetValueOrDefault(key, defaultValue);
            Assert.Equal(defaultValue, retrivedValue2);
        }

        #region -- Private Methods --

        private static Dictionary<string, string> GetDictionary()
        {
            var Dict = new Dictionary<string, string>();

            var allLetters = Enumerable.Range('a', 'z')
                .Select(letter => ((char)letter).ToString())
                .ToList();

            foreach (var letter in allLetters)
            {
                Dict.Add(letter, letter);
            }

            return Dict;
            
        }

        #endregion

    }
}
