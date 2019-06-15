using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WideWorldImporters.Core.CoreServices.Interfaces;
using WideWorldImporters.Core.Helpers;
using Xunit;

namespace WideWorldImporters.Tests.Services
{

    /// <summary>
    /// 
    /// </summary>
    public class RedisServiceTest
    {

        private IRedisService _redisService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="redisService"></param>
        public RedisServiceTest(IRedisService redisService)
        {
            _redisService = redisService;
        }

        /// <summary>
        /// 
        /// </summary>
        [Theory]
        [InlineData(10)]
        public async Task TestCacheAsync(int numberOfTests)
        {
            
            foreach(var current in Enumerable.Range(0, numberOfTests))
            {
                var randomNumber = IntHelpers.GetRandomNumber(current * 100);
                var key = "random" + current.ToString();

                await _redisService.SetAsync<int>(key, randomNumber);

                Assert.Equal(randomNumber, await _redisService.GetAsync<int>(key));

            }


        }

    }

}
