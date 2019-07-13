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

namespace WideWorldImporters.Tests.ServiceUnitTests
{

    /// <summary>
    /// Tests for RedisService
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
            Mock<IDistributedCache> distributedCacheMock = new Mock<IDistributedCache>();

            _redisService = new RedisService(distributedCacheMock.Object);
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
        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task TestMultipleKeys(int numberOfKeysToTest)
        {
            var randomKeys = Enumerable.Range(0, numberOfKeysToTest)
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

        

    }

}
