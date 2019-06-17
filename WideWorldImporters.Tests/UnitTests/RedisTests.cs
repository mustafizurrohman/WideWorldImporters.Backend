using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using WideWorldImporters.Core.CoreServices.Implementation;
using WideWorldImporters.Core.CoreServices.Interfaces;
using WideWorldImporters.Core.Enumerations;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Core.Helpers;
using Xunit;

namespace WideWorldImporters.Tests.UnitTests
{

    /// <summary>
    /// Tests for RedisSerivce
    /// </summary>
    public class RedisTests
    {
        private readonly IRedisService _redisService;

        /// <summary>
        /// Constructor
        /// </summary>
        public RedisTests()
        {
            // Mock the IDistributed Cache
            IDistributedCache distributedCacheMock = new Mock<IDistributedCache>().Object;

            _redisService = new RedisService(distributedCacheMock);
        }

        /// <summary>
        /// Tests if a key is stored properly
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestIfKeyIsProperlyStored()
        {
            var randomInt = IntHelpers.GetRandomNumber(100);

            await _redisService.SetAsync<int>(RedisKeys.Default, randomInt);

            Assert.True(_redisService.Exist(RedisKeys.Default));
        }

        /// <summary>
        /// Tests a random number of keys 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestMultipleKeys()
        {
            var randomKeys = Enumerable.Range(0, 100)
                .Select(number => StringHelpers.GetRandomString())
                .ToList();

            foreach (var randomKey in randomKeys)
            {
                await _redisService.SetAsync<string>(randomKey, StringHelpers.GetRandomString());
            }

            randomKeys = randomKeys.Shuffle().ToList();

            foreach(var key in randomKeys)
            {
                Assert.True(_redisService.Exist(key));
            }

        }

        /// <summary>
        /// Test with inputs
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("a1", "b1")]
        [InlineData("a2", "b2")]
        [InlineData("a3", "b3")]
        public async Task TestKeysWithInput(string key, string value)
        {
            await _redisService.SetAsync<string>(key, value);

            Assert.True(_redisService.Exist(key));
        }

    }

}
